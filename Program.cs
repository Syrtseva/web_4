using System;
using System.Reflection;

public class Person
{
    // Поля з різними типами даних та модифікаторами доступу
    private string name;
    protected int age;
    public string address;
    internal double height;
    private bool isEmployed;

    // Конструктор
    public Person(string name, int age, string address, double height, bool isEmployed)
    {
        this.name = name;
        this.age = age;
        this.address = address;
        this.height = height;
        this.isEmployed = isEmployed;
    }

    // Методи
    public void DisplayInfo()
    {
        Console.WriteLine($"Name: {name}, Age: {age}, Address: {address}, Height: {height}, Employed: {isEmployed}");
    }

    protected string GetEmploymentStatus()
    {
        return isEmployed ? "Employed" : "Unemployed";
    }

    internal void ChangeAddress(string newAddress)
    {
        address = newAddress;
    }
}

class Program
{
    static void Main(string[] args)
    {
        // 1. Створення об'єкту класу Person
        Person person = new Person("John", 30, "123 Main St", 5.9, true);

        // 2. Демонстрація роботи з Type і TypeInfo
        Type type = typeof(Person); //отримання типу класу
        TypeInfo typeInfo = type.GetTypeInfo();//отримання додаткової інформації про клас, такої як ім'я та простір імен.
        Console.WriteLine($"Class Name: {typeInfo.Name}");
        Console.WriteLine($"Namespace: {typeInfo.Namespace}");

        // 3. Демонстрація роботи з MemberInfo
        MemberInfo[] members = type.GetMembers();//Отримані всі члени класу та виведені їх типи і імена.
        Console.WriteLine("\nMembers:");
        foreach (var member in members)
        {
            Console.WriteLine($"- {member.MemberType}: {member.Name}");
        }

        // 4. Демонстрація роботи з FieldInfo (клас, який містить інформацію про поля в класах)
        FieldInfo[] fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);//Отримані всі приватні поля класу з відповідним флагом (не публічні, не статичні) 
        Console.WriteLine("\nFields:");
        foreach (var field in fields)
        {
            Console.WriteLine($"Field Name: {field.Name}, Field Type: {field.FieldType}");
            // Зчитування значення поля
            var value = field.GetValue(person);
            Console.WriteLine($"Field Value: {value}");
        }

        // 5. Демонстрація роботи з MethodInfo
        MethodInfo methodInfo = type.GetMethod("DisplayInfo");//отримання доступу до методу DisplayInfo та виклику його через рефлексію.
        Console.WriteLine("\nInvoking method using Reflection:");
        methodInfo.Invoke(person, null); // Виклик методу через рефлексію

        // Виклик захищеного методу (отримання доступу до методу, який є захищеним)
        MethodInfo protectedMethod = type.GetMethod("GetEmploymentStatus", BindingFlags.NonPublic | BindingFlags.Instance);
        string employmentStatus = (string)protectedMethod.Invoke(person, null);//виклик захищеного методу
        Console.WriteLine($"Employment Status: {employmentStatus}");

        // Виклик внутрішнього методу (отримання доступу до методу, який є внутрішнім)
        MethodInfo internalMethod = type.GetMethod("ChangeAddress", BindingFlags.NonPublic | BindingFlags.Instance);
        internalMethod.Invoke(person, new object[] { "456 Another St" });// виклик методу та передача нової адреси
        methodInfo.Invoke(person, null); // Перевірка зміни адреси
    }
}
