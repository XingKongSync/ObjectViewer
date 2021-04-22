﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ObjectViewer
{
    public class ObjectNode
    {
        private object _bindObject;
        private List<ObjectNode> _childern;


        public string Name { get; set; }
        
        public string Value { get; set; }

        public List<ObjectNode> Children
        {
            get
            {
                if (_childern == null && _bindObject != null)
                {
                    _childern = DecodeObject(_bindObject);
                }
                return _childern;
            }
        }

        public ObjectNode() { }

        public ObjectNode(object bindObject) 
        {
            _bindObject = bindObject;
        }

        public static List<ObjectNode> DecodeObject(object target)
        {
            List<ObjectNode> result = new List<ObjectNode>();
            if (target == null)
                return result;

            Type targetType = target.GetType();
            if ((targetType.IsValueType && targetType.IsPrimitive) || targetType.Equals(typeof(string)) || targetType.IsEnum)
            {
                //处理数值类型
                result.Add(new ObjectNode() { Name = "Value", Value = target.ToString() });
            }
            else if (target is ICollection collection)
            {
                //处理集合类型
                if (target is IDictionary dictionary)
                {
                    //处理字典类型
                    foreach (var key in dictionary.Keys)
                    {
                        object value = dictionary[key];
                        result.Add(new ObjectNode(value)
                        {
                            Name = key.ToString(),
                            Value = value.ToString()
                        });
                    }
                }
                else
                {
                    //处理其他集合类型
                    int i = 0;
                    foreach (object value in collection)
                    {
                        result.Add(new ObjectNode(value)
                        {
                            Name = i.ToString(),
                            Value = value.ToString()
                        });
                        i++;
                    }
                }
            }
            else
            {
                //处理对象、结构体
                //获取公共属性
                PropertyInfo[] properties = targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                if (properties != null)
                {
                    foreach (PropertyInfo property in properties)
                    {
                        object value = property.GetValue(target, null);
                        Type vType = value?.GetType();
                        if ((vType?.IsValueType != false && vType?.IsPrimitive != false) || typeof(string).Equals(vType) || vType?.IsEnum == true)
                        {
                            result.Add(new ObjectNode()
                            {
                                Name = property.Name,
                                Value = value?.ToString() ?? "null"
                            });
                        }
                        else
                        {
                            result.Add(new ObjectNode(value)
                            {
                                Name = property.Name,
                                Value = value?.ToString() ?? "null"
                            });
                        }
                    }
                }

                //获取公共字段
                FieldInfo[] fields = targetType.GetFields(BindingFlags.Public | BindingFlags.Instance);
                if (fields != null)
                {
                    foreach (FieldInfo field in fields)
                    {
                        object value = field.GetValue(target);
                        Type fType = value?.GetType();
                        if (fType?.IsValueType != false && fType.IsPrimitive != false)
                        {
                            result.Add(new ObjectNode()
                            {
                                Name = field.Name,
                                Value = value?.ToString()
                            });
                        }
                        else
                        {
                            result.Add(new ObjectNode(value)
                            {
                                Name = field.Name,
                                Value = value?.ToString()
                            });
                        }
                    }
                }
            }

            return result;
        }

    }
}
