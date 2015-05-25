Generics advantages and disadvantage
1. Performance
2. Type safety
3. Binary code reuse
4. Code bloat
5. Naming guidelines

Performance
One of the big advantages of generics is performance. 
Using value types with non-generic collection classes results in boxing and unboxing when the value type is converted to a reference type, and vice versa.

Value types are stored on the stack, whereas reference types are stored on the heap. C# classes are reference types; structs are value types. 

int row = 10;
Object obj= row;
class Person {
	private string empNo;
	private string name;
}

var list = new ArrayList();
list.Add(44); //boxing - converte a value type to a reference type

int i1 = (int)list[0]; //unboxing - converte a reference type to a value type
foreach(int i2 in list)
{
	Console.WriteLine(i2); //unboxing
}

Boxing and unboxing are easy to use but have a big performance impact, especially when iterating through many items.

Instead of using objects, the List<T> class from the namespace System.Collections.Generic enables you to define the type when it is used. In the example here, the generic type of the List<T> class is defined as int, so the int type is used inside the class that is generated dynamically from the JIT compiler. Boxing and unboxing no longer happen:
var list = new List<int>();
list.Add(44); // no boxing — value types are stored in the List<int>
int i1 = list[0]; // no unboxing, no cast needed
foreach (int i2 in list) 
{
	Console.WriteLine(i2);
}

Type safety
Another feature of generics is type safety. As with the ArrayList class, if objects are used, any type can be added to this collection. The following example shows adding an integer, a string, and an object of type MyClass to the collection of type ArrayList:
var list = new ArrayList(); 
list.Add(44); 
list.Add("mystring"); 
list.Add(new MyClass());
If this collection is iterated using the following foreach statement, which iterates using integer elements, the compiler accepts this code. However, because not all elements in the collection can be cast to an int, a runtime exception will occur:
foreach (int i in list) 
{
	Console.WriteLine(i); 
}
Errors should be detected as early as possible. With the generic class List<T>, the generic type T defines what types are allowed. With a definition of List<int>, only integer types can be added to the collection. The compiler doesn’t compile this code because the Add() method has invalid arguments:
var list = new List<int>();
list.Add(44);
list.Add("mystring"); // compile time error 
list.Add(new MyClass()); // compile time error

Binary Code reuse
Generics enable better binary code reuse. A generic class can be defined once and can be instantiated with many different types. Unlike C++ templates, it is not necessary to access the source code.
For example, here the List<T> class from the namespace System.Collections.Generic is instantiated
with an int, a string, and a MyClass type:
	var list = new List<int>();
	list.Add(44);

	var stringList = new List<string>();
	stringList.Add("mystring");

	var MyClassList = new List<MyClass>();
	MyClassList.Add(new MyClass());
Generic type can be defined in one language and used from any other .NET language.

Code bloat
	You might be wondering how much code is created with generics when instantiating them with different specific types. Because a generic class definition goes into the assembly, instantiating generic classes
	with specific types doesn’t duplicate these classes in the IL code. However, when the generic classes are compiled by the JIT compiler to native code, a new class for every specific value type is created. Reference types share all the same implementation of the same native class. This is because with reference types, only a 4-byte memory address (with 32-bit systems) is needed within the generic instantiated class to reference a reference type. Value types are contained within the memory of the generic instantiated class; and because every value type can have different memory requirements, a new class for every value type is instantiated.

Naming guidelines
	If generics are used in the program, it helps when generic types can be distinguished from non-generic types. Here are naming guidelines for generic types:
➤ ➤
➤
Generic type names should be prefixed with the letter T.
If the generic type can be replaced by any class because there’s no special requirement, and only one
generic type is used, the character T is good as a generic type name: 
public class List<T> { }
public class LinkedList<T> { }
If there’s a special requirement for a generic type (for example, it must implement an interface or derive from a base class), or if two or more generic types are used, descriptive names should be used for the type names:
public delegate void EventHandler<TEventArgs>(object sender, TEventArgs e);
public delegate TOutput Converter<TInput, TOutput>(TInput from); public class SortedList<TKey, TValue> { }