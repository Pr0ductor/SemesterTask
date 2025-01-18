using TemplateEngine.UnitTests.Models;

namespace TemplateEngine.UnitTests
{
    [TestFixture]
    public class HtmlTemplateEngineTests
    {
        [Test]
        public void Render_ValidTemplateAndData_ReturnHtml()
        {
            // Arrange
            IHtmlTemplateEngine engine = new HtmlTemplateEngine();
            var template = "Привет, {name}! Как дела?";
            var data = "Вася";

            // Act
            var result = engine.Render(template, data);

            // Assert
            Assert.AreEqual("Привет, Вася! Как дела?", result);
        }

        [Test]
        public void Render_ValidObject_ReturnHtml()
        {
            // Arrange
            IHtmlTemplateEngine engine = new HtmlTemplateEngine();
            var student = new Student { Id = 1, Name = "Вася" };
            var template = "Ура вы поступили, {name}! Ваш номер студенческого билета: {id}";


            // Act
            var result = engine.Render(template, student);

            // Assert
            Assert.AreEqual("Привет, Вася! Как дела?", result);
        }
    }
}
