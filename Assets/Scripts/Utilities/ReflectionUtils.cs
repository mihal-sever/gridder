using System;
using System.Linq.Expressions;
using System.Reflection;

public class PropertyWrapper
{
    public Func<object, object> Getter;
    public Action<object, object> Setter;
}

public static class ReflectionUtils
{
    public static PropertyWrapper BuildPropertyWrapper(PropertyInfo propertyInfo)
    {
        return new PropertyWrapper()
        {
            Getter = BuildPropertyGetter(propertyInfo),
            Setter = BuildPropertySetter(propertyInfo)
        };
    }

    public static Func<object, object> BuildPropertyGetter(PropertyInfo propertyInfo)
    {
        Type targetType = propertyInfo.DeclaringType;
        ParameterExpression targetParameter = Expression.Parameter(typeof(object));
        UnaryExpression targetParameterExpression = Expression.Convert(targetParameter, targetType);

        MethodInfo getter = propertyInfo.GetGetMethod(true);
        MethodCallExpression getterCallExpression = Expression.Call(targetParameterExpression, getter);
        UnaryExpression finalGetter = Expression.Convert(getterCallExpression, typeof(object));

        Expression<Func<object, object>> lambda = Expression.Lambda<Func<object, object>>(finalGetter, targetParameter);

        return lambda.Compile();
    }

    public static Action<object, object> BuildPropertySetter(PropertyInfo propertyInfo)
    {
        Type targetType = propertyInfo.DeclaringType;
        ParameterExpression targetParameter = Expression.Parameter(typeof(object));
        UnaryExpression targetParameterExpression = Expression.Convert(targetParameter, targetType);

        Type valueType = propertyInfo.PropertyType;
        ParameterExpression valueParameter = Expression.Parameter(typeof(object));
        UnaryExpression valueParameterExpression = Expression.Convert(valueParameter, valueType);

        MethodInfo setter = propertyInfo.GetSetMethod(true);
        MethodCallExpression setterCallExpression = Expression.Call(targetParameterExpression, setter, valueParameterExpression);
        UnaryExpression finalSetter = Expression.Convert(setterCallExpression, typeof(void));

        Expression<Action<object, object>> lambda = Expression.Lambda<Action<object, object>>(finalSetter, targetParameter, valueParameter);

        return lambda.Compile();
    }
}