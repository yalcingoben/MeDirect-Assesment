using ExchangeTrader.Redis;
using Moq;
using StackExchange.Redis;
using System.Text.Json;

namespace ExchangeTrader.Caching.Redis
{
    public class RedisCacheServiceTest
    {
        private readonly Mock<IConnectionMultiplexer> _redisCon;
        private readonly Mock<IDatabase> _cache;

        public RedisCacheServiceTest()
        {
            _redisCon = new Mock<IConnectionMultiplexer>();
            _cache = new Mock<IDatabase>();
        }

        [Fact]
        public async void Clear_Should_Be_Clear_When_Key_Was_Given()
        {
            //Arrange
            var key = Guid.NewGuid().ToString();

            _cache.Setup(x => x.KeyDeleteAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>())).ReturnsAsync(true);
            _redisCon.Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object?>())).Returns(_cache.Object);
            //Act
            var cacheService = new RedisCacheService(_redisCon.Object);
            await cacheService.Clear(key, CancellationToken.None);
            //Assert
            _cache.Verify(x=> x.KeyDeleteAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()), Times.Once());
        }

        [Fact]
        public async void GetOrAddAsync_Should_Get_All_Datas_When_Stored_In_Cache()
        {
            //Arrange
            var key = Guid.NewGuid().ToString();
            var expireTime = TimeSpan.FromSeconds(10);

            var redisValue = JsonSerializer.SerializeToUtf8Bytes("Test");

            _cache.Setup(x => x.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>())).ReturnsAsync(()=> redisValue);
            _redisCon.Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object?>())).Returns(_cache.Object);
            Func<Task<string>> func = GetKey;
            Task<string> GetKey() { return Task.FromResult("Test"); }

            //Act
            var cacheService = new RedisCacheService(_redisCon.Object);
            var result = await cacheService.GetOrAddAsync<string>(key, GetKey, expireTime, CancellationToken.None);
            //Assert
            _cache.Verify(x => x.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()), Times.Once());
            Assert.Equal("Test", result);
        }

        [Fact]
        public async void GetOrAddAsync_Should_Get_All_Datas_Through_Action_Method_When_Data_Is_Not_Found_In_Cache()
        {
            //Arrange
            var key = Guid.NewGuid().ToString();
            var expireTime = TimeSpan.FromSeconds(10);

            var redisValue = JsonSerializer.SerializeToUtf8Bytes("Test");

            _cache.Setup(x => x.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>())).ReturnsAsync(() => RedisValue.Null);
            _cache.Setup(x => x.StringSetAsync(It.IsAny<RedisKey>(), 
                It.IsAny<RedisValue>(), 
                It.IsAny<TimeSpan?>(), 
                It.IsAny<bool>(), 
                It.IsAny<When>(), 
                It.IsAny<CommandFlags>())).ReturnsAsync(()=> true);
            _redisCon.Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object?>())).Returns(_cache.Object);
            Func<Task<string>> func = GetKey;
            Task<string> GetKey() { return Task.FromResult("Test"); }

            //Act
            var cacheService = new RedisCacheService(_redisCon.Object);
            var result = await cacheService.GetOrAddAsync<string>(key, GetKey, expireTime, CancellationToken.None);
            //Assert
            _cache.Verify(x => x.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()), Times.Once());
            Assert.Equal("Test", result);
        }

        [Fact]
        public async void GetValueAsync_Should_Get_Value_When_Data_Is_Stored_In_Cache()
        {
            //Arrange
            var key = Guid.NewGuid().ToString();
            var redisValue = "Test";

            _cache.Setup(x => x.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>())).ReturnsAsync(() => redisValue);            
            _redisCon.Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object?>())).Returns(_cache.Object);
            
            //Act
            var cacheService = new RedisCacheService(_redisCon.Object);
            var result = await cacheService.GetValueAsync(key, CancellationToken.None);
            //Assert
            _cache.Verify(x => x.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()), Times.Once());
            Assert.Equal("Test", result);
        }

        [Fact]
        public async void SetValueAsync_Should_Be_Send_To_Cache_When_Data_Is_Given()
        {
            //Arrange
            var key = Guid.NewGuid().ToString();
            var expireTime = TimeSpan.FromSeconds(10);
            var redisValue = "Test";

            _cache.Setup(x => x.StringSetAsync(It.IsAny<RedisKey>(),
                It.IsAny<RedisValue>(),
                It.IsAny<TimeSpan?>(),
                It.IsAny<bool>(),
                It.IsAny<When>(),
                It.IsAny<CommandFlags>())).ReturnsAsync(() => true);
            _redisCon.Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object?>())).Returns(_cache.Object);

            //Act
            var cacheService = new RedisCacheService(_redisCon.Object);
            var result = await cacheService.SetValueAsync(key, redisValue, expireTime, CancellationToken.None);
            //Assert            
            Assert.True(result);
        }

        [Fact]
        public void GetOrAdd_Should_Get_All_Datas_When_Stored_In_Cache()
        {
            //Arrange
            var key = Guid.NewGuid().ToString();
            var expireTime = TimeSpan.FromSeconds(10);

            var redisValue = JsonSerializer.SerializeToUtf8Bytes("Test");

            _cache.Setup(x => x.StringGet(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>())).Returns(redisValue);
            _redisCon.Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object?>())).Returns(_cache.Object);
            Func<string> func = GetKey;
            string GetKey() { return "Test"; }

            //Act
            var cacheService = new RedisCacheService(_redisCon.Object);
            var result = cacheService.GetOrAdd<string>(key, GetKey, expireTime, CancellationToken.None);
            //Assert
            _cache.Verify(x => x.StringGet(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()), Times.Once());
            Assert.Equal("Test", result);
        }

        [Fact]
        public void GetOrAdd_Should_Get_All_Datas_Through_Action_Method_When_Data_Is_Not_Found_In_Cache()
        {
            //Arrange
            var key = Guid.NewGuid().ToString();
            var expireTime = TimeSpan.FromSeconds(10);

            var redisValue = JsonSerializer.SerializeToUtf8Bytes("Test");

            _cache.Setup(x => x.StringGet(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>())).Returns(RedisValue.Null);
            _cache.Setup(x => x.StringSet(It.IsAny<RedisKey>(),
                It.IsAny<RedisValue>(),
                It.IsAny<TimeSpan?>(),
                It.IsAny<bool>(),
                It.IsAny<When>(),
                It.IsAny<CommandFlags>())).Returns(true);
            _redisCon.Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object?>())).Returns(_cache.Object);
            Func<string> func = GetKey;
            string GetKey() { return "Test"; }

            //Act
            var cacheService = new RedisCacheService(_redisCon.Object);
            var result = cacheService.GetOrAdd<string>(key, GetKey, expireTime, CancellationToken.None);
            //Assert
            _cache.Verify(x => x.StringGet(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()), Times.Once());
            Assert.Equal("Test", result);
        }

    }
}