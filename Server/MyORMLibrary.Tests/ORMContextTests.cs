using System.Data;
using System.Data.SqlClient;
using Moq;
using MyORMLibrary.Tests.Models;
using NUnit.Framework;


namespace MyORMLibrary.Tests;

public class ORMContextTests
{
    private ORMContext<UserInfo> _dbContext;

    [Test]
    public void GetById_When_()
    {
        var _dbConnection = new Mock<IDbConnection>();
        var _dbCommand = new Mock<IDbCommand>();
        var _dbDataReader = new Mock<IDataReader>();
        var _dataParametr = new Mock<IDbDataParameter>();
        var _dataParametrCollection = new Mock<IDataParameterCollection>();

        _dbContext = new ORMContext<UserInfo>(_dbConnection.Object);
    
        var userId = 1;
        var userInfo = new UserInfo()
        {
            Id = 1,
            Age = 20,
            Email = "exaple@test.com",
            Name = "Иванов Иван Иванович",
            Gender = 1
        };

        var data = new List<Dictionary<string, object>>
        {
            new Dictionary<string, object>
            {
                {"Id", userInfo.Id},
                {"Age", userInfo.Age},
                {"Email", userInfo.Email},
                {"Name", userInfo.Name},
                {"Gender", userInfo.Gender}

            }
        };

        _dbDataReader.SetupSequence(r => r.Read())
            .Returns(true)
            .Returns(false);
        _dbDataReader.Setup(r => r["Id"]).Returns(userInfo.Id);
        _dbDataReader.Setup(r => r["Age"]).Returns(userInfo.Age);
        _dbDataReader.Setup(r => r["Email"]).Returns(userInfo.Email);
        _dbDataReader.Setup(r => r["Name"]).Returns(userInfo.Name);
        _dbDataReader.Setup(r => r["Gender"]).Returns(userInfo.Gender);

        _dbCommand.Setup(c => c.ExecuteReader()).Returns(_dbDataReader.Object);
        _dbCommand.Setup(c => c.CreateParameter()).Returns(_dataParametr.Object);
        _dbCommand.Setup(c => c.Parameters).Returns(_dataParametrCollection.Object);
        _dataParametrCollection.Setup(pc => pc.Add(It.IsAny<Object>())).Returns(userId);
        _dbConnection.Setup(c => c.CreateCommand()).Returns(_dbCommand.Object);

        var result = _dbContext.ReadById(userId);
        
        Assert.IsNotNull(result);
        Assert.AreEqual(userInfo.Id, result.Id);
        Assert.AreEqual(userInfo.Age, result.Age);
        Assert.AreEqual(userInfo.Email, result.Email);
        Assert.AreEqual(userInfo.Name, result.Name);
        Assert.AreEqual(userInfo.Gender, result.Gender);
    }
}