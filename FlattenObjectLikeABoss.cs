void Main()
{
	List<object> list = new List<object>();
	var results = new ArrayList();
	foreach (var o in list)
	{
		var result = Flatten(o);
		results.Add(result);
	}
	results.Dump();
}

private static ExpandoObject Flatten(object o, string key = "", ExpandoObject result = null, int depth = 0)
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
			Flatten(obj, $"{key}[{iter++}]", result, depth++);
		}
		return result;
	}
	if (!string.IsNullOrWhiteSpace(key))
		key += ".";
	foreach (System.Reflection.PropertyInfo fi in o.GetType().GetProperties())
	{
		var obj = fi.GetValue(o, null);
		Flatten(obj, $"{key}{fi.Name}", result, depth++);
	}
	return result;
}
