﻿using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace Grove.Infrastructure
{
  public delegate object ParameterlessCtor();

  public delegate void FieldSetter(object target, object value);

  public static class Activation
  {
    // Faster way to dynamicly create objects
    // http://rogeralsing.com/2008/02/28/linq-expressions-creating-objects/
    public static ParameterlessCtor GetParameterlessCtor(this Type type)
    {
      var ctor = type.GetConstructor(
        BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, null,
        CallingConventions.Any,
        Type.EmptyTypes, null);

      if (ctor == null)
        return null;

      var newExpression = Expression.New(ctor);
      var lambda = Expression.Lambda(typeof (ParameterlessCtor), newExpression);
      var compiled = (ParameterlessCtor) lambda.Compile();
      return compiled;
    }

    public static Func<object, object> GetGetter(this FieldInfo fieldInfo)
    {
      var targetParameter = Expression.Parameter(typeof (object), "target");
      var convertedParameter = Expression.Convert(targetParameter, fieldInfo.DeclaringType);
      var field = Expression.Field(convertedParameter, fieldInfo);
      var convertedField = Expression.Convert(field, typeof (object));
      var activator = Expression.Lambda(convertedField, targetParameter).Compile();

      return (Func<object, object>) activator;
    }

    public static Action<object, object> GetSetter(this FieldInfo fieldInfo)
    {
      // lambda expressions cannot write readonly fields so emit must be used instead
      // http://stackoverflow.com/questions/9930459/how-to-initialize-a-struct-in-array-with-readonly-values-using-expression-trees

      var setMethod = new DynamicMethod(
        fieldInfo.Name,
        typeof (void),
        new[] {typeof (object), typeof (object)},
        fieldInfo.DeclaringType);

      var generator = setMethod.GetILGenerator();
      var local = generator.DeclareLocal(fieldInfo.DeclaringType);

      generator.Emit(OpCodes.Ldarg_0);
      generator.Emit(OpCodes.Castclass, fieldInfo.DeclaringType);
      generator.Emit(OpCodes.Stloc_0, local);
      generator.Emit(OpCodes.Ldloc_0, local);
      generator.Emit(OpCodes.Ldarg_1);

      if (fieldInfo.FieldType.IsValueType)
      {
        generator.Emit(OpCodes.Unbox_Any, fieldInfo.FieldType);
      }
      else
      {
        generator.Emit(OpCodes.Castclass, fieldInfo.FieldType);
      }

      generator.Emit(OpCodes.Stfld, fieldInfo);
      generator.Emit(OpCodes.Ret);

      return (Action<object, object>) setMethod.CreateDelegate(typeof (Action<object, object>));
    }
  }
}