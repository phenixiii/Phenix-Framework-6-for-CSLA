using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Phenix.Core.Reflection
{
  /// <summary>
  /// 动态执行构建函数的委托函数
  /// </summary>
  public delegate object DynamicCtorDelegate();
  /// <summary>
  /// 动态执行函数的委托函数
  /// </summary>
  public delegate object DynamicMethodDelegate(object target, object[] args);

  /// <summary>
  /// 动态执行get函数的委托函数
  /// </summary>
  public delegate object DynamicMemberGetDelegate(object target);

  /// <summary>
  /// 动态执行set函数的委托函数
  /// </summary>
  public delegate void DynamicMemberSetDelegate(object target, object arg);

  /// <summary>
  /// 动态工厂
  /// </summary>
  public static class DynamicFactory
  {
    /// <summary>
    /// 动态执行构建函数
    /// </summary>
    public static DynamicCtorDelegate CreateConstructor(ConstructorInfo constructor)
    {
      if (constructor == null)
        throw new ArgumentNullException("constructor");
      if (constructor.GetParameters().Length > 0)
        throw new ArgumentException("不支持多参数的构建函数", "constructor");

      Expression body = Expression.New(constructor);
      if (constructor.DeclaringType.IsValueType)
        body = Expression.Convert(body, typeof(object));
      return Expression.Lambda<DynamicCtorDelegate>(body).Compile();
    }

    /// <summary>
    /// 动态执行函数
    /// </summary>
    public static DynamicMethodDelegate CreateMethod(MethodInfo method)
    {
      if (method == null)
        throw new ArgumentNullException("method");

      ParameterExpression targetExpression = Expression.Parameter(typeof(object));
      ParameterExpression parametersExpression = Expression.Parameter(typeof(object[]));
      ParameterInfo[] parameters = method.GetParameters();
      Expression[] callParamExpressions = new Expression[parameters.Length];
      for (int i = 0; i < parameters.Length; i++)
        callParamExpressions[i] = Expression.Convert(Expression.ArrayIndex(parametersExpression, Expression.Constant(i)), parameters[i].ParameterType);
      Expression instanceExpression = Expression.Convert(targetExpression, method.DeclaringType);
      Expression bodyExpression = parameters.Length > 0
        ? Expression.Call(instanceExpression, method, callParamExpressions)
        : Expression.Call(instanceExpression, method);
      if (method.ReturnType == typeof(void))
      {
        LabelTarget target = Expression.Label(typeof(object));
        ConstantExpression nullRefExpression = Expression.Constant(null);
        bodyExpression = Expression.Block(bodyExpression, Expression.Return(target, nullRefExpression), Expression.Label(target, nullRefExpression));
      }
      else if (method.ReturnType.IsValueType)
        bodyExpression = Expression.Convert(bodyExpression, typeof(object));
      return Expression.Lambda<DynamicMethodDelegate>(bodyExpression, targetExpression, parametersExpression).Compile();
    }

    /// <summary>
    /// 动态执行属性的get函数
    /// </summary>
    public static DynamicMemberGetDelegate CreatePropertyGetter(PropertyInfo property)
    {
      if (property == null)
        throw new ArgumentNullException("property");

      if (!property.CanRead)
        return null;

      ParameterExpression targetExpression = Expression.Parameter(typeof(object));
      Expression bodyExpression = Expression.Property(Expression.Convert(targetExpression, property.DeclaringType), property);
      if (property.PropertyType.IsValueType)
        bodyExpression = Expression.Convert(bodyExpression, typeof(object));
      return Expression.Lambda<DynamicMemberGetDelegate>(bodyExpression, targetExpression).Compile();
    }

    /// <summary>
    /// 动态执行属性的set函数
    /// </summary>
    public static DynamicMemberSetDelegate CreatePropertySetter(PropertyInfo property)
    {
      if (property == null)
        throw new ArgumentNullException("property");

      if (!property.CanWrite)
        return null;

      ParameterExpression targetExpression = Expression.Parameter(typeof(object));
      ParameterExpression valueExpression = Expression.Parameter(typeof(object));
      Expression bodyExpression = Expression.Assign(Expression.Property(Expression.Convert(targetExpression, property.DeclaringType), property), Expression.Convert(valueExpression, property.PropertyType));
      return Expression.Lambda<DynamicMemberSetDelegate>(bodyExpression, targetExpression, valueExpression).Compile();
    }

    /// <summary>
    /// 动态执行字段的get函数
    /// </summary>
    public static DynamicMemberGetDelegate CreateFieldGetter(FieldInfo field)
    {
      if (field == null)
        throw new ArgumentNullException("field");

      ParameterExpression targetExpression = Expression.Parameter(typeof(object));
      Expression bodyExpression = Expression.Field(Expression.Convert(targetExpression, field.DeclaringType), field);
      if (field.FieldType.IsValueType)
        bodyExpression = Expression.Convert(bodyExpression, typeof(object));
      return Expression.Lambda<DynamicMemberGetDelegate>(bodyExpression, targetExpression).Compile();
    }

    /// <summary>
    /// 动态执行字段的set函数
    /// </summary>
    public static DynamicMemberSetDelegate CreateFieldSetter(FieldInfo field)
    {
      if (field == null)
        throw new ArgumentNullException("field");

      ParameterExpression targetExpression = Expression.Parameter(typeof(object));
      ParameterExpression valueExpression = Expression.Parameter(typeof(object));
      Expression bodyExpression = Expression.Assign(Expression.Field(Expression.Convert(targetExpression, field.DeclaringType), field), Expression.Convert(valueExpression, field.FieldType));
      return Expression.Lambda<DynamicMemberSetDelegate>(bodyExpression, targetExpression, valueExpression).Compile();
    }
  }
}