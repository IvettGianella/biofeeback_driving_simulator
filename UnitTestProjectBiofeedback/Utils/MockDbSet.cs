using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProjectBiofeedback.Utils
{
    public static class MockDbSet
    {
        public static Mock<DbSet<T>> CreateMockDbSet<T>(List<T> data) where T : class
        {
            var mock = new Mock<DbSet<T>>();
            var queryData = data.AsQueryable();
            //mock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryData.Provider);

            mock.As<IDbAsyncEnumerable<T>>()
            .Setup(m => m.GetAsyncEnumerator())
            .Returns(new TestDbAsyncEnumerator<T>(queryData.GetEnumerator()));
            mock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(new TestDbAsyncQueryProvider<T>(queryData.Provider));

            mock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryData.Expression);
            mock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryData.ElementType);
            mock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryData.GetEnumerator());

            Type type = typeof(T);
            var pk = type.GetProperty("Id");

            mock.Setup(x => x.Add(It.IsAny<T>())).Returns((T x) =>
            {
                var count = data.Count();
                pk.SetValue(x, count + 1);

                data.Add(x);
                return x;
            });

            mock.Setup(x => x.Remove(It.IsAny<T>())).Returns((T x) =>
            {
                data.Remove(x);
                return x;
            });

            mock.Setup(x => x.Find(It.IsAny<object[]>())).Returns((object[] id) =>
            {
                return queryData.FirstOrDefault(u => (int)pk.GetValue(u) == (int)id[0]);
            });

            return mock;
        }
    }
}
