using System.Data;
using System.Data.SqlClient;
using AllProject.UnitTests.Models;
using HttpServerLibrary.Configurations;
using HttpServerLibrary.Models;
using MyORMLibrary;

namespace AllProject.UnitTests;

[TestFixture]
public class ORMContextTests
{
    private IDbConnection _dbConnection;
    private ORMContext<Entity> _context;

    /// <summary>
    /// Настройка перед каждым тестом.
    /// </summary>
    [SetUp]
    public void Setup()
    {
        // Настройте соединение с базой данных для тестирования
        _dbConnection = new SqlConnection(AppConfig.GetInstance().ConnectionStrings["DefaultConnection"]);
        _context = new ORMContext<Entity>(_dbConnection);

        // Очистите таблицу перед каждым тестом
        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = "DELETE FROM Entitys";
            _dbConnection.Open();
            command.ExecuteNonQuery();
            _dbConnection.Close();
        }
    }

    /// <summary>
    /// Очистка после каждого теста.
    /// </summary>
    [TearDown]
    public void TearDown()
    {
        _dbConnection.Dispose();
    }

    /// <summary>
    /// Тест на создание сущности и вставку её в базу данных.
    /// </summary>
    [Test]
    public void Create_ShouldInsertEntityIntoDatabase()
    {
        // Arrange
        var entity = new Entity { Name = "TestEntity", Value = 123 };

        // Act
        _context.Create(entity);

        // Assert
        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = "SELECT COUNT(*) FROM Entitys WHERE Name = 'TestEntity' AND Value = 123";
            _dbConnection.Open();
            var count = (int)command.ExecuteScalar();
            _dbConnection.Close();
            Assert.AreEqual(1, count);
        }
    }

    /// <summary>
    /// Тест на получение всех сущностей из базы данных.
    /// </summary>
    [Test]
    public void GetAll_ShouldReturnAllEntities()
    {
        // Arrange
        var entity1 = new Entity { Name = "Entity1", Value = 1 };
        var entity2 = new Entity { Name = "Entity2", Value = 2 };
        _context.Create(entity1);
        _context.Create(entity2);

        // Act
        var result = _context.GetAll();

        // Assert
        Assert.AreEqual(2, result.Count);
        Assert.IsTrue(result.Exists(e => e.Name == "Entity1" && e.Value == 1));
        Assert.IsTrue(result.Exists(e => e.Name == "Entity2" && e.Value == 2));
    }

    /// <summary>
    /// Тест на получение первой сущности, соответствующей предикату.
    /// </summary>
    [Test]
    public void FirstOrDefault_ShouldReturnFirstEntityMatchingPredicate()
    {
        // Arrange
        var entity1 = new Entity { Name = "Entity1", Value = 1 };
        var entity2 = new Entity { Name = "Entity2", Value = 2 };
        _context.Create(entity1);
        _context.Create(entity2);

        // Act
        var result = _context.FirstOrDefault(e => e.Value == 1);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Entity1", result.Name);
        Assert.AreEqual(1, result.Value);
    }

    /// <summary>
    /// Тест на получение сущности по имени.
    /// </summary>
    [Test]
    public void GetByName_ShouldReturnEntityWithGivenName()
    {
        // Arrange
        var entity = new Entity { Name = "TestEntity", Value = 123 };
        _context.Create(entity);

        // Act
        var result = _context.GetByTitle("TestEntity");

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("TestEntity", result.Name);
        Assert.AreEqual(123, result.Value);
    }
}