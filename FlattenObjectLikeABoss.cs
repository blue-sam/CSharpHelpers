void Main()
{
	var list = new[]{
	new
	{
		HouseHold = new { name = "Wilsons", id = Guid.NewGuid() },
		People = new[] { new { firstName = "Jarvis",
			Addresses = new[] {
				new { Line1 = "123", State = "Utah"},
				new { Line1 = "345", State = "Arizona"}
			} } }
	},
	new
	{
		HouseHold = new { name = "Famburgs", id = Guid.NewGuid() },
		People = new[] { new { firstName = "Sam",
			Addresses = new[] {
				new { Line1 = "123", State = null as string}
			} } }
	}};
	Flattener.Flatten(list).Dump();
}

static class Flattener
{
	public static object Flatten(object value)
	{
		if (value is IEnumerable)
		{
			return Flatten((IEnumerable<object>)value);
		}
		var temp = FlattenInt(value);
		return temp;
	}
	public static IEnumerable<object> Flatten(IEnumerable list)
	{
		var results = new List<object>();
		foreach (var o in list)
		{
			object result = Flatten(o);
			results.Add(result);
		}
		return results;
	}

	private static ExpandoObject FlattenInt(object o, string key = "", ExpandoObject result = null, int depth = 0)
	{
		result = result ?? new ExpandoObject();
		dynamic dyn = result;
		IDictionary<string, object> temp = dyn;
		if (o == null || o.GetType().IsValueType || o is string)
		{
			temp[key] = o;
			return result;
		}
		if (depth > 15)
			return result;

		if (o is IEnumerable)
		{
			var arr = new List<object>();
			var iter = 0;
			foreach (object obj in (IEnumerable)o)
			{
				FlattenInt(obj, $"{key}[{iter++}]", result, depth++);
			}
			return result;
		}
		if (!string.IsNullOrWhiteSpace(key))
			key += ".";
		foreach (System.Reflection.PropertyInfo fi in o.GetType().GetProperties())
		{
			var obj = fi.GetValue(o, null);
			FlattenInt(obj, $"{key}{fi.Name}", result, depth++);
		}
		return result;
	}
}
