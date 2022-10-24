using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Phenix.Core.Reflection
{
  /// <summary>
  /// 反射TTarget
  /// </summary>
  /// <typeparam name="TTarget">被反射的类</typeparam>
  public static class Reflect<TTarget>
  {
    /// <summary>
    /// 获取表达式中描述的方法
    /// </summary>
    /// <param name="method">表达式</param>
    /// <returns>方法</returns>
    /// <exception cref="ArgumentNullException">The <paramref name="method"/> is null.</exception>
    /// <exception cref="ArgumentException">The <paramref name="method"/> is not a lambda expression or it does not represent a method invocation.</exception>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
    public static MethodInfo GetMethod(Expression<Action<TTarget>> method)
    {
      return GetMethodInfo(method);
    }

    /// <summary>
    /// 获取表达式中描述的方法
    /// </summary>
    /// <param name="method">表达式</param>
    /// <returns>方法</returns>
    /// <exception cref="ArgumentNullException">The <paramref name="method"/> is null.</exception>
    /// <exception cref="ArgumentException">The <paramref name="method"/> is not a lambda expression or it does not represent a method invocation.</exception>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
    public static MethodInfo GetMethod<T1>(Expression<Action<TTarget, T1>> method)
    {
      return GetMethodInfo(method);
    }

    /// <summary>
    /// 获取表达式中描述的方法
    /// </summary>
    /// <param name="method">表达式</param>
    /// <returns>方法</returns>
    /// <exception cref="ArgumentNullException">The <paramref name="method"/> is null.</exception>
    /// <exception cref="ArgumentException">The <paramref name="method"/> is not a lambda expression or it does not represent a method invocation.</exception>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
    public static MethodInfo GetMethod<T1, T2>(Expression<Action<TTarget, T1, T2>> method)
    {
      return GetMethodInfo(method);
    }

    /// <summary>
    /// 获取表达式中描述的方法
    /// </summary>
    /// <param name="method">表达式</param>
    /// <returns>方法</returns>
    /// <exception cref="ArgumentNullException">The <paramref name="method"/> is null.</exception>
    /// <exception cref="ArgumentException">The <paramref name="method"/> is not a lambda expression or it does not represent a method invocation.</exception>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
    public static MethodInfo GetMethod<T1, T2, T3>(Expression<Action<TTarget, T1, T2, T3>> method)
    {
      return GetMethodInfo(method);
    }

    private static MethodInfo GetMethodInfo(Expression method)
    {
      if (method == null) 
        throw new ArgumentNullException("method");

      LambdaExpression lambda = method as LambdaExpression;
      if (lambda == null) 
        throw new ArgumentException("不是表达式", "method");
      if (lambda.Body.NodeType != ExpressionType.Call) 
        throw new ArgumentException("不存在方法调用", "method");
      return ((MethodCallExpression)lambda.Body).Method;
    }

    /// <summary>
    /// 获取表达式中描述的属性
    /// </summary>
    /// <param name="property">表达式</param>
    /// <returns>属性</returns>
    /// <exception cref="ArgumentNullException">The <paramref name="property"/> is null.</exception>
    /// <exception cref="ArgumentException">The <paramref name="property"/> is not a lambda expression or it does not represent a property access.</exception>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
    public static PropertyInfo GetProperty(Expression<Func<TTarget, object>> property)
    {
      PropertyInfo info = GetMemberInfo(property) as PropertyInfo;
      if (info == null)
        throw new ArgumentException("表达式中不存在属性", "property");
      return info;
    }

    /// <summary>
    /// 获取表达式中描述的属性
    /// </summary>
    /// <typeparam name="P">属性类型</typeparam>
    /// <param name="property">表达式</param>
    /// <returns>属性</returns>
    /// <exception cref="ArgumentNullException">The <paramref name="property"/> is null.</exception>
    /// <exception cref="ArgumentException">The <paramref name="property"/> is not a lambda expression or it does not represent a property access.</exception>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
    public static PropertyInfo GetProperty<P>(Expression<Func<TTarget, P>> property)
    {
      PropertyInfo info = GetMemberInfo(property) as PropertyInfo;
      if (info == null)
        throw new ArgumentException("表达式中不存在属性", "property");
      return info;
    }

    /// <summary>
    /// 获取表达式中描述的字段
    /// </summary>
    /// <param name="field">表达式</param>
    /// <returns>字段</returns>
    /// <exception cref="ArgumentNullException">The <paramref name="field"/> is null.</exception>
    /// <exception cref="ArgumentException">The <paramref name="field"/> is not a lambda expression or it does not represent a field access.</exception>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
    public static FieldInfo GetField(Expression<Func<TTarget, object>> field)
    {
      FieldInfo fieldInfo = GetMemberInfo(field) as FieldInfo;
      if (fieldInfo == null)
        throw new ArgumentException("表达式中不存在字段", "field");
      return fieldInfo;
    }

    private static MemberInfo GetMemberInfo(Expression member)
    {
      if (member == null)
        throw new ArgumentNullException("member");

      LambdaExpression lambda = member as LambdaExpression;
      if (lambda == null)
        throw new ArgumentException("不是表达式", "member");

      MemberExpression memberExpr = null;
      if (lambda.Body.NodeType == ExpressionType.Convert)
        memberExpr = ((UnaryExpression)lambda.Body).Operand as MemberExpression;
      else if (lambda.Body.NodeType == ExpressionType.MemberAccess)
        memberExpr = lambda.Body as MemberExpression;
      if (memberExpr == null) 
        throw new ArgumentException("不存在成员访问", "member");
      return memberExpr.Member;
    }
  }
}
