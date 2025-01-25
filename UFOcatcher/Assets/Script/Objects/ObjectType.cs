namespace Utility
{
	public class Objects
	{
		// To handle unique names that differ from the enum (e.g.: MyObject -> "My Object")
		public static string GetObjectName(ObjectType objectType)
		{
			switch (objectType)
			{
				case ObjectType.Amongus:
					return "Amogus";
				default:
					return objectType.ToString();
			}
		}

		public enum ObjectType
		{
			Box,
			Wheat,
			Cow,
			Amongus
		}
	}
}