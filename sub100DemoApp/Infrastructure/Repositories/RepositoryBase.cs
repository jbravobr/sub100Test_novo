using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SQLiteNetExtensions.Extensions;

namespace sub100DemoApp
{
	public class RepositoryBase<T> : IRepositoryBase<T> where T : EntityBase, new()
	{
		public object _lock = new object();

		public RepositoryBase()
		{
			if (App.AppConnection == null)
				App.AppConnection = DBContext.Instance;

			CheckDatabase();
		}

		void CheckDatabase()
		{
			App.AppConnection.DropTable<Cliente>();
			App.AppConnection.DropTable<Endereco>();
			App.AppConnection.DropTable<Imovei>();
			App.AppConnection.DropTable<Imovel>();

			App.AppConnection.CreateTable<Cliente>(SQLite.CreateFlags.None);
			App.AppConnection.CreateTable<Endereco>(SQLite.CreateFlags.None);
			App.AppConnection.CreateTable<Imovei>(SQLite.CreateFlags.None);
			App.AppConnection.CreateTable<Imovel>(SQLite.CreateFlags.None);
		}

		public bool Any()
		{
			lock (_lock)
				return App.AppConnection.Table<T>().Any();
		}

		public void Delete(T TEntity)
		{
			lock (_lock)
				App.AppConnection.Delete(TEntity);
		}

		public List<T> GetAll()
		{
			lock (_lock)
				return App.AppConnection.GetAllWithChildren<T>(null, recursive: true);
		}

		public List<T> GetAllByPredicate(Expression<Func<T, bool>> predicate)
		{
			lock (_lock)
				return App.AppConnection.GetAllWithChildren(predicate, recursive: true);
		}

		public T GetById(int pkId)
		{
			lock (_lock)
				return App.AppConnection.GetWithChildren<T>(pkId, recursive: true);
		}

		public T GetByPredicate(Expression<Func<T, bool>> predicate)
		{
			lock (_lock)
			{
				var entity = App.AppConnection.Table<T>().FirstOrDefault(predicate);
				if (entity != null)
					return App.AppConnection.GetWithChildren<T>(entity.Id, recursive: true);

				return new T();
			}
		}

		public void Insert(T TEntity)
		{
			lock (_lock)
				App.AppConnection.InsertOrReplaceWithChildren(TEntity, recursive: true);
		}

		public void Update(T TEntity)
		{
			lock (_lock)
				App.AppConnection.UpdateWithChildren(TEntity);
		}

		public int InsertAndReturnInsertedPK(T TEntity)
		{
			lock (_lock)
			{
				App.AppConnection.InsertOrReplaceWithChildren(TEntity);
				var last = App.AppConnection.GetAllWithChildren<T>(null, recursive: true);
				return last.Last().Id;
			}
		}
	}
}