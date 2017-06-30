﻿using System;
using System.Linq;
using System.Reflection;
using LibAtem.Commands;
using LibAtem.Serialization;
using Xunit;

namespace LibAtem.Test.Util
{
    internal class RandomCommandGenerator
    {
        public static readonly Random random = new Random();

        public static T Create<T>() where T : ICommand
        {
            return (T) Create(typeof(T));
        }

        public static ICommand Create(Type t)
        {
            ICommand cmd = (ICommand) Activator.CreateInstance(t);
            foreach (PropertyInfo prop in t.GetProperties())
            {
                // If prop cannot be deserialized, then ignore
                if (!prop.CanWrite)
                    continue;

                // Generate random prop value
                IRandomGeneratorAttribute attr = prop.GetCustomAttributes().OfType<IRandomGeneratorAttribute>().FirstOrDefault();
                if (attr != null)
                {
                    prop.SetValue(cmd, attr.GetRandom(random));
                    continue;
                }

                // If prop is an enum, then take a random value
                if (prop.PropertyType.GetTypeInfo().IsEnum)
                {
                    Array values = Enum.GetValues(prop.PropertyType);
                    prop.SetValue(cmd, values.GetValue(random.Next(values.Length)));
                    continue;
                }

                Assert.True(false, string.Format("Missing generator attribute for property: {0}", prop.Name));
            }
            return cmd;
        }

        public static void AssertAreTheSame(ICommand orig, ICommand decoded)
        {
            PropertyInfo[] props = orig.GetType().GetProperties();
            foreach (PropertyInfo prop in props)
            {
                object origVal = prop.GetValue(orig);
                object decodedVal = prop.GetValue(decoded);

                SerializableAttributeBase attr = prop.GetCustomAttribute<SerializableAttributeBase>();
                if (attr != null)
                {
                    Assert.True(attr.AreEqual(origVal, decodedVal), 
                        string.Format("{0}.{1} does not match. Orig:{2}, Decoded:{3}", orig.GetType().Name, prop.Name, origVal, decodedVal));
                    continue;
                }

                Assert.True(Equals(origVal, decodedVal),
                    string.Format("{0}.{1} does not match. Orig:{2}, Decoded:{3}", orig.GetType().Name, prop.Name, origVal, decodedVal));
            }
        }
    }
}
