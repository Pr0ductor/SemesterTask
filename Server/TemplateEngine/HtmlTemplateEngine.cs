using System.Collections;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;


namespace TemlateEngine;

/// <summary>
/// Синтаксис шаблонизатора:
/// {{Value}} выделение переменных
/// {%if<Условие>%} Действие если условие истинно {%else%} действие если условие ложно {%/if%}
/// {%for% ITEM %in% COLLECTION}Действия с ITEM {%/for%}
/// </summary>
public class HtmlTemplateEngine : IHtmlTemplateEngine
{
    /// <summary>
    /// Рендерит шаблон, заменяя строку на данные.
    /// </summary>
    /// <param name="template">Шаблон.</param>
    /// <param name="str">Строка для замены.</param>
    /// <param name="data">Данные для замены.</param>
    /// <returns>Отрендренный шаблон.</returns>
    public string Render(string template, string str, string data)
    {
        return template.Replace(str, data);
    }

    /// <summary>
    /// Рендерит шаблон из файла, заменяя переменные на значения из объекта.
    /// </summary>
    /// <param name="fileInfo">Информация о файле шаблона.</param>
    /// <param name="obj">Объект с данными для замены.</param>
    /// <returns>Отрендренный шаблон.</returns>
    public string Render(FileInfo fileInfo, object obj)
    {
        var templatePath = fileInfo.FullName;
        if (File.Exists(templatePath))
        {
            return Render(template: File.ReadAllText(templatePath), obj);
        }
        else
        {
            Console.WriteLine($"File {templatePath} not found");
            throw new FileNotFoundException($"File {templatePath} not found");
        }
    }

    /// <summary>
    /// Рендерит шаблон из потока, заменяя переменные на значения из объекта.
    /// </summary>
    /// <param name="stream">Поток с шаблоном.</param>
    /// <param name="obj">Объект с данными для замены.</param>
    /// <returns>Отрендренный шаблон.</returns>
    public string Render(Stream stream, object obj)
    {
        if (stream.CanRead)
        {
            var content = new StreamReader(stream).ReadToEnd();
            return Render(content, obj);
        }
        else
        {
            throw new InvalidOperationException("Stream can't be read");
        }
    }

    /// <summary>
    /// Рендерит шаблон, заменяя переменные на значения из объекта.
    /// </summary>
    /// <param name="template">Шаблон.</param>
    /// <param name="obj">Объект с данными для замены.</param>
    /// <returns>Отрендренный шаблон.</returns>
    public string Render(string template, object obj)
    {
        var properties = obj.GetType().GetProperties();
        var result = template;
        try
        {
            result = HandleLoops(result, properties, obj);
            result = HandleConditionals(result, properties, obj);
            result = HandleVariables(result, obj);
        }
        catch (ArgumentNullException e)
        {
            Console.WriteLine(e.Message);
            return e.Message;
        }
        return result;
    }

    /// <summary>
    /// Обрабатывает переменные в шаблоне.
    /// </summary>
    /// <param name="template">Шаблон.</param>
    /// <param name="obj">Объект с данными для замены.</param>
    /// <returns>Шаблон с замененными переменными.</returns>
    public string HandleVariables(string template, object obj)
    {
        if (obj is null) throw new ArgumentNullException("obj is null");

        var result = template;
        var regex = new Regex(@"{{(.*?)}}");

        result = regex.Replace(result, match =>
        {
            var propertyPath = match.Groups[1].Value; // Например, "Genre1.Name"
            var value = GetNestedPropertyValue(obj, propertyPath);
            return value ?? " "; // Если значение не найдено, оставить как есть
        });

        return result;
    }

    /// <summary>
    /// Получает значение вложенного свойства объекта.
    /// </summary>
    /// <param name="obj">Объект.</param>
    /// <param name="propertyPath">Путь к свойству.</param>
    /// <returns>Значение свойства или null, если свойство не найдено.</returns>
    private string GetNestedPropertyValue(object obj, string propertyPath)
    {
        if (obj == null || string.IsNullOrWhiteSpace(propertyPath)) return null;

        var currentObject = obj;
        var properties = propertyPath.Split('.'); // Разбиваем "Genre1.Name" на ["Genre1", "Name"]
        foreach (var property in properties)
        {
            if (currentObject == null) return null;

            var type = currentObject.GetType();
            var propertyInfo = type.GetProperty(property, BindingFlags.Public | BindingFlags.Instance);

            if (propertyInfo == null) return null;

            currentObject = propertyInfo.GetValue(currentObject);
        }

        return currentObject?.ToString(); // Возвращаем значение свойства или null
    }

    /// <summary>
    /// Обрабатывает условные конструкции в шаблоне.
    /// </summary>
    /// <param name="template">Шаблон.</param>
    /// <param name="properties">Свойства модели.</param>
    /// <param name="model">Модель с данными.</param>
    /// <returns>Шаблон с обработанными условными конструкциями.</returns>
    private string HandleConditionals(string template, PropertyInfo[] properties, object model)
    {
        if (model is null) throw new ArgumentNullException("model is null");
        var pattern = @"{%if<(.+?)>%}(.*?)({%else%}(.*?))?{%/if%}";
        var regex = new Regex(pattern, RegexOptions.Singleline);
        return new string(regex.Replace(template, match =>
        {
            var conditional = match.Groups[1].Value.Trim();
            var content = match.Groups[2].Value.Trim();
            var elseContent = match.Groups[3].Success ? match.Groups[4].Value.Trim() : null;

            if (ProcessCondition(conditional, properties, model))
            {
                return Render(content, model);
            }
            if (elseContent != null)
            {
                return Render(elseContent, model);
            }
            return string.Empty;
        }));
    }

    /// <summary>
    /// Обрабатывает условие и возвращает результат.
    /// </summary>
    /// <param name="condition">Условие.</param>
    /// <param name="properties">Свойства модели.</param>
    /// <param name="model">Модель с данными.</param>
    /// <returns>Результат условия.</returns>
    private bool ProcessCondition(string condition, PropertyInfo[] properties, object model)
    {
        if (model is null) throw new ArgumentNullException("model is null");
        if (properties is null || properties.Length == 0) throw new ArgumentNullException("model properties are null");
        foreach (var property in properties)
        {
            var value = property.GetValue(model) is not bool ?
                $"'{property.GetValue(model)}'" : property.GetValue(model).ToString();
            var pattern = $@"{{{{{property.Name}}}}}";
            condition = Regex.Replace(condition, pattern, value, RegexOptions.IgnoreCase);
        }

        condition = condition.Replace("==", "=")
            .Replace("&&", "AND")
            .Replace("||", "OR")
            .Replace("!=", "<>");
        try
        {
            var result = (bool)new System.Data.DataTable().Compute(condition, string.Empty);
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    /// <summary>
    /// Обрабатывает циклы в шаблоне.
    /// </summary>
    /// <param name="template">Шаблон.</param>
    /// <param name="properties">Свойства модели.</param>
    /// <param name="model">Модель с данными.</param>
    /// <returns>Шаблон с обработанными циклами.</returns>
    private string HandleLoops(string template, PropertyInfo[] properties, object model)
    {
        var pattern = @"{%for%(.+?)%in%(.+?)}(.*?){%/for%}";
        var regex = new Regex(pattern, RegexOptions.Singleline);

        return regex.Replace(template, match =>
        {
            var itemName = match.Groups[1].Value.Trim(); // Например, "movie"
            var collectionName = match.Groups[2].Value.Trim(); // Например, "movies"
            var content = match.Groups[3].Value; // Шаблон для каждого элемента

            // Найти коллекцию в свойствах модели
            var collectionProperty = properties.FirstOrDefault(p => p.Name.Equals(collectionName, StringComparison.OrdinalIgnoreCase));
            if (collectionProperty == null)
            {
                throw new ArgumentException($"Collection '{collectionName}' not found in model.");
            }

            // Получить значение коллекции
            var collection = collectionProperty.GetValue(model) as IEnumerable;
            if (collection == null)
            {
                throw new ArgumentException($"Property '{collectionName}' is not a valid collection.");
            }

            var loopResult = new StringBuilder();

            foreach (var value in collection)
            {
                if (value == null) continue;

                string renderedItem;

                if (value.GetType().IsPrimitive || value is string)
                {
                    // Для простых типов (например, string) заменяем {{movie}} на значение
                    renderedItem = content.Replace($"{{{{{itemName}}}}}", value.ToString());
                }
                else
                {
                    // Для объектов заменяем {{movie.Property}} на значения их свойств
                    var itemTemplate = content.Replace("{{" + itemName + ".", "{{");
                    renderedItem = Render(itemTemplate, value);
                }

                loopResult.Append(renderedItem);
            }

            return loopResult.ToString();
        });
    }

    /// <summary>
    /// Проверяет, находится ли строка внутри цикла.
    /// </summary>
    /// <param name="content">Содержимое шаблона.</param>
    /// <param name="check">Строка для проверки.</param>
    /// <returns>True, если строка находится внутри цикла; иначе false.</returns>
    private bool CheckStringInLoop(string content, string check)
    {
        if (content.IndexOf("{%for%") == -1) return false;
        return content.IndexOf("{%for%") < content.IndexOf(check) &&
               content.IndexOf("{%/for%}") > content.IndexOf(check);
    }

    /// <summary>
    /// Проверяет, находится ли строка внутри условной конструкции.
    /// </summary>
    /// <param name="content">Содержимое шаблона.</param>
    /// <param name="check">Строка для проверки.</param>
    /// <returns>True, если строка находится внутри условной конструкции; иначе false.</returns>
    private bool CheckStringInCond(string content, string check)
    {
        if (content.IndexOf("{%if") == -1) return false;
        return content.IndexOf("{%if") < content.IndexOf(check) &&
               content.IndexOf("{%/if%}") > content.IndexOf(check);
    }

    /// <summary>
    /// Пример обработки исключений.
    /// </summary>
    void exampleException()
    {
        try
        {
            throw new InvalidOperationException("hjhkhjhjkhhk");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
        }
        catch (Exception ex)
        {
            // Обработка других исключений
        }
    }
}