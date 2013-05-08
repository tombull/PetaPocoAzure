namespace PetaPocoAzure.Internal
{
	static class Singleton<T> where T : new()
	{
		public static T Instance = new T();
	}

}
