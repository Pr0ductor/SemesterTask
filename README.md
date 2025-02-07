# FirstSemesterTask


## Описание
Server - это полноценный Web-Server, демонстрирующий базовый функционал современного веб-приложения.

## Содержание
- [Установка](#установка)
- [Использование](#использование)
- [Примеры](#примеры)
- [Вклад](#вклад)
- [Лицензия](#лицензия)
- [Контакты](#контакты)

## Установка
1. Клонируйте репозиторий:
   ```sh
   git clone https://github.com/Pr0ductor/SemesterTask

2. Перейдите в директорию проекта:
   ```sh
   cd SemesterTask

3. Установите зависимости:
   ```sh
   dotnet restore



## Использование

1. Скомпилируйте проект:
   ```sh
   dotnet build

2. Запустите проект:
   ```sh
   dotnet run

3. Откройте браузер и перейдите по адресу:
   ```
    http://localhost:8888


## Примеры
1. Создайте базу данных PersonDB. Выполните все SQL скрипты перед запуском программы (SQL commands) (проект работает через докерфайл) (SQL commands копируйте в режиме code для удобства)
   ```sql
   create table Users
   (
    Id       int identity
        primary key,
    Login    nvarchar(50) not null
        unique,
    Password nvarchar(50) not null,
    Email    nvarchar(50) not null
        unique
   )
   create table Movies
   (
    id           int identity,
    title        nvarchar(255),
    release_year int,
    duration     int,
    description  nvarchar(max),
    poster_url   nvarchar(255),
    director     nvarchar(max),
    actors       nvarchar(max),
    URL_video    nvarchar(max),
    country      nvarchar(max),
    genre        nvarchar(max)
   )
   create table Entities
   (
    Id    int identity
        primary key,
    Name  nvarchar(255) not null,
    Value int           not null,
    Login nvarchar(255)
   )
   create table Admins
   (
    Id       int identity
        primary key,
    Login    varchar(50) not null
        unique,
    Password varchar(50) not null,
    Email    varchar(50) not null
   )
   INSERT INTO PersonDB.dbo.Admins (Login, Password, Email) VALUES (N'qwerty', N'qwerty', N'qwerty');

   INSERT INTO PersonDB.dbo.Movies (title, release_year, duration, description, poster_url, director, actors, URL_video, country, genre) VALUES (N'Соник 3', 2024, 110, N'Продолжение удивительных приключений любимых героев с детства мира сверхскоростного ёжика Соник выходит на экраны. Персонажи всемирно известной видеоигры снова покидают пределы просто компьютеров. Новые герои, новые враги, но всё та же полюбившаяся вселенная уже ждут зрителей.', N'../assets/posts/2024-09/a9b6c35ad2_sonik-3.webp', N'Джефф Фаулер', N'Брендан Мерфи, Barry Calvert, Alexsia Lana Cheung, Бола Окун, Тору Накахара, Йорма Такконе, Элайла Браун, Джеймс Волк, Реджи Баниго, Джеймс Марсден', N'https://dud.allarknow.online/?token_movie=4bfbc370e7bad4f8fb8a4c8baadd13&token=10b16a40f5793e2d02d06265c13912', N'США, Япония', N'Комедия, Боевик, Приключения, Фэнтези, Фантастика, Семейный');
   INSERT INTO PersonDB.dbo.Movies (title, release_year, duration, description, poster_url, director, actors, URL_video, country, genre) VALUES (N'Муфаса: Король Лев', 2024, 120, N'В местности, расположенной далеко от Скалы Прайда, родился будущий отец Симбы Муфаса. Маленький львёнок рано лишился родителей. Однажды, попав под сильный дождь, малыш не смог совладать с разбушевавшимися потоками воды, которые унесли его прочь от родных мест. Заблудившегося и чудом выжившего случайно обнаруживает взрослая львица с детёнышем по прозвищу Шрам. Пушистики начинают вместе играть...', N'../assets/posts/2024-09/mufasa_-korol-lev.webp', N'Барри Дженкинс', N'Тусо Мбеду, Сет Роген, Билли Айкнер, Блу Айви Картер, Фолаке Оловофойеку, Мадс Миккельсен, Neon Wings, Аника Нони Роуз, Бриэль Рэнкинс, Тео Сомолу', N'https://dud.allarknow.online/?token_movie=4bfbc370e7bad4f8fb8a4c8baadd13&token=10b16a40f5793e2d02d06265c13912', N'США', N'Драма, Приключения, Фэнтези, Семейный, Мюзикл');
   INSERT INTO PersonDB.dbo.Movies (title, release_year, duration, description, poster_url, director, actors, URL_video, country, genre) VALUES (N'Крейвен-охотник', 2024, 127, N'Русские аристократы, бежавшие за границу после падения империи, выросли в тревожное время. Сергей Кравинов был сыном иммигранта, переехавшего в Америку в 1917 году. Мать ребенка умерла, когда мальчику не исполнилось и трех лет. Из-за потери жены отец еще строже отнёсся к Сереже, вкладывая в него все надежды на восстановление былой славы семьи. С детства ему внушали, что главное — не показывать...', N'../assets/posts/2024-09/krejven-ohotnik.webp', N'Джей Си Чендор', N'Аарон Тейлор-Джонсон, Ариана Дебоуз, Фред Хекинджер, Алессандро Нивола, Кристофер Эбботт, Рассел Кроу, Юрий Колокольников, Леви Миллер, Том Рид, Билли Бэррэтт', N'https://dud.allarknow.online/?token_movie=4bfbc370e7bad4f8fb8a4c8baadd13&token=10b16a40f5793e2d02d06265c13912', N'США, Великобритания, Исландия, Канада', N'Боевик, Приключения, Триллер, Фантастика');
   INSERT INTO PersonDB.dbo.Movies (title, release_year, duration, description, poster_url, director, actors, URL_video, country, genre) VALUES (N'Моана 2', 2024, 100, N'Моане предстоит долгий путь в неизведанные земли, после приглашения от предков-искателей. Девушка смогла пройти через множество испытаний и познакомится с удивительными созданиями. Также красавице удалось осознать своё прошлое и завести дружбу...', N'../assets/posts/2024-09/scale_1200.webp', N'Дэвид Деррик мл.', N'Роуз Матафео, Халиси Ламберт-Цуда, Tofiga Fepulea''i, Алан Тьюдик, Bentley Pupuhi-Fernandez, Джералд Файтала Рэмси, Ата Джонсон, Николь Шерзингер, Дуэйн Джонсон, Авимаи Фрейзер', N'https://dud.allarknow.online/?token_movie=4bfbc370e7bad4f8fb8a4c8baadd13&token=10b16a40f5793e2d02d06265c13912', N'США, Канада', N'Комедия, Приключения, Фэнтези, Семейный, Мюзикл');
   INSERT INTO PersonDB.dbo.Movies (title, release_year, duration, description, poster_url, director, actors, URL_video, country, genre) VALUES (N'Злая: Сказка о ведьме Запада', 2024, 160, N'В Стране Жевунов с жизнерадостной и игривой женой проживал губернатор. И вот однажды ее развлечения зашли так далеко, что привели к интрижке с заезжим торговцем, в результате чего она родила дочь. Был нюанс: любовник вместе с утехами угощал ее эликсиром, в результате появившийся на свет ребенок имел кожу...', N'../assets/posts/2024-09/zlaja_-skazka-o-vedme-zapada.webp', N'Джон М. Чу', N'Идина Мензел, Робин Берри, Клайв Кнеллер, Питер Динклэйдж, Jasmine McIvor, Joey Unitt, Dereke Oladele, Sienna-Rose Amer, Кристин Ченоуэт, Дэбби Куруп', N'https://dud.allarknow.online/?token_movie=4bfbc370e7bad4f8fb8a4c8baadd13&token=10b16a40f5793e2d02d06265c13912', N'США, Великобритания, Канада, Япония, Исландия', N'Боевик, Приключения, Мелодрама, Фэнтези, Семейный, Мюзикл');
   INSERT INTO PersonDB.dbo.Movies (title, release_year, duration, description, poster_url, director, actors, URL_video, country, genre) VALUES (N'Властелин колец: Война рохирримов', 2024, 134, N'История происходит в прошлом. Король Рохана, Хельм, девятый по счету владыка, воюет с племенем дунландцев, и их вождем Фреком. Он посягает на трон, и хочет обвенчать своего сына Вульфа, на дочери Хельма. Главная крепость Средиземья в опасности. Туземцы настолько осмелели, что выдвинули требования, с которыми повелитель не согласился. Он ударил назойливого Фрека так сильно, что тот, вскоре умер...', N'../assets/posts/2024-09/vlastelin-kolec_-vojna-rohirrimov.webp', N'Кэндзи Камияма', N'Шон Дули, Ронни Джатти, Daniel Denova, Нил МакКаул, Лоррейн Эшборн, Джонатан Магнанти, Шэйн Бартл, Джошуа Майлз, Доминик Монахэн, Найджел Пилкингтон', N'https://dud.allarknow.online/?token_movie=4bfbc370e7bad4f8fb8a4c8baadd13&token=10b16a40f5793e2d02d06265c13912', N'США, Япония', N'Боевик, Фэнтези');
   INSERT INTO PersonDB.dbo.Movies (title, release_year, duration, description, poster_url, director, actors, URL_video, country, genre) VALUES (N'Приключения Паддингтона 3', 2024, 106, N'Став любимым пушистым домочадцем и законным британцем в дружной семье Браунов очаровательный медведь Паддингтон счастлив и одаривается внимательной заботой, но мечты не проходят о родине и вечнозеленых джунглях Перу. С момента получения письма отзывчивый путешественник отправляется в гости...', N'../assets/posts/2024-09/prikljuchenija-paddingtona-3.webp', N'Дугал Уилсон', N'Бен Уишоу, Хью Бонневилль, Эмили Мортимер, Мадлен Харрис, Сэмюэл Джослин, Джули Уолтерс', N'https://dud.allarknow.online/?token_movie=4bfbc370e7bad4f8fb8a4c8baadd13&token=10b16a40f5793e2d02d06265c13912', N'Великобритания, США, Франция, Япония', N'Детектив, Комедия, Приключения, Семейный, Фэнтези');
   INSERT INTO PersonDB.dbo.Movies (title, release_year, duration, description, poster_url, director, actors, URL_video, country, genre) VALUES (N'Миссия: Красный', 2024, 123, N'Любимый праздник детства – Рождество под угрозой срыва. Весь Северный Полюс охватила паника и беспокойство. Таинственным образом пропал главный волшебник – Санта Клаус Святой Николай. По неизвестным причинам он более известен как Красный. Отважный шериф, ответственный за безопасность Санты...', N'../assets/posts/2024-09/missija_-krasnyj.webp', N'Джейк Кэздан', N'Clayton Cooper, Рейнальдо Фаберль, Микаэль Турек, Adam Boyer, Otis Kasdan, Эшли Домангу, Мэри Элизабет Эллис, Джи-Род, Samantha Benson, Шэйн Коста', N'https://dud.allarknow.online/?token_movie=4bfbc370e7bad4f8fb8a4c8baadd13&token=10b16a40f5793e2d02d06265c13912', N'США, Канада', N'Комедия, Боевик, Приключения, Детектив, Фэнтези');
   INSERT INTO PersonDB.dbo.Movies (title, release_year, duration, description, poster_url, director, actors, URL_video, country, genre) VALUES (N'Гладиатор 2', 2024, 148, N'Власть, интриги, коварство - всё это неотъемлемые черты древнего города Рим, столицы могущественной империи, где толпы людей пируют, смотря как в агонии бьются рабы на огромной арене знаменитого Колизея. Главный герой - Луций, племянник деспотичного императора Комода, в ранней юности сбегает из Вечного города...', N'../assets/posts/2024-09/gladiator-2.webp', N'Ридли Скотт', N'Nisrine Machat, Maud Oulhen, Дин Фаган, Line Ancel, Юваль Гонен, Дэвид Ганли, Фред Хекинджер, Mouaiz El Outmany, Igor Badnjar, Ричард Кац', N'https://dud.allarknow.online/?token_movie=4bfbc370e7bad4f8fb8a4c8baadd13&token=10b16a40f5793e2d02d06265c13912', N'США, Великобритания, Канада, Мальта, Марокко', N'Драма, Боевик, Приключения, История');
   INSERT INTO PersonDB.dbo.Movies (title, release_year, duration, description, poster_url, director, actors, URL_video, country, genre) VALUES (N'Дюна: Пророчество', 2024, 720, N'«Бене Гессерит» стремятся контролировать важнейшие политические и династические цепочки по всему космосу. Они использовали свои навыки, чтобы манипулировать королевскими семьями, заключать важные браки и эффективно продвигаясь к созданию семьи, которая могла бы в дальнейшем привести к появлению Квисатца...', N'../assets/posts/2024-11/djuna-prorochestvo-poster.webp', N'Джон Камерон', N'Паркер Сойерс, Трэвис Фиммел, Люси Расселл, Tom Swale, Ерин Ха, Лаура Ховард, Ифа Хайндс, Марк Эдди, Джоди Мэй, Sarah Oliver-Watts', N'https://dud.allarknow.online/?token_movie=4bfbc370e7bad4f8fb8a4c8baadd13&token=10b16a40f5793e2d02d06265c13912', N'США, Канада, Венгрия', N'Драма, Боевик, Приключения, Фантастика');

   INSERT INTO PersonDB.dbo.Users (Login, Password, Email) VALUES (N'asd', N'123', N'asd');
   INSERT INTO PersonDB.dbo.Users (Login, Password, Email) VALUES (N'asdf', N'qwerty', N'denis2@gmail.com');
   

3. Перейдите по эндпоинту movies. Перед собой вы видите каталог фильмов. на данном этапе
   открывается любой фильм, но перед этим необходимо зарегестрироваться.

4. В правом верхнем углу вы найдёте кнопку "Войти".
   Вы можете зайти под одним из зарегистрированных пользователей.

5. После входа вы будете перенаправлены на главную страницу.
   Ваш логин будет отображаться вместо кнопки "Войти" и теперь вы можете перейти на любую доступную страницу с фильмом.
   Попробуйте нажать...

6. введя admlogin открывается вход на админ пользователя

7. Пройдя верификацию вы попадёте на страницу "панель администратора".
   Здесь вы можете добавлять и удалять элементы таблиц бд.


## Вклад

   Форкните проект
   Создайте ветку для вашей функции (git checkout -b feature/AmazingFeature)
   Сделайте коммит (git commit -m 'Add some SemesterTask')
   Сделайте пуш в ветку (git push origin feature/SemesterTask)
   Откройте Pull Request

## Лицензия

   Распространяется под лицензией MIT. Смотрите LICENSE для получения дополнительной информации.
   

## Контакты

Denis - @Pr0ductor - denisiyus@gmail.com
