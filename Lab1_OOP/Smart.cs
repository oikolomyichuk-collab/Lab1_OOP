using System;

public class Smart
{
    private string brand;
    private string model;
    private int ozyGB;
    private int cameraMPx;

    public string Brand
    {
        get { return brand; }
        set
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length < 3 || value.Length > 12)
                throw new ArgumentException("Бренд має бути від 3 до 12 символів!");
            brand = value;
        }
    }

    public string Model
    {
        get { return model; }
        set
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length < 2 || value.Length > 12)
                throw new ArgumentException("Модель має бути від 2 до 12 символів!");
            model = value;
        }
    }

    public int OzyGB
    {
        get { return ozyGB; }
        set
        {
            if (value < 0 || value > 512)
                throw new ArgumentException("ОЗУ повинно бути від 0 до 512 ГБ!");
            ozyGB = value;
        }
    }

    public int CameraMPx
    {
        get { return cameraMPx; }
        set
        {
            if (value <= 0 || value > 264)
                throw new ArgumentException("Камера повинна бути від 1 до 264 Мп!");
            cameraMPx = value;
        }
    }

    public SmartphoneType Type { get; set; }

    public Smart(string brand, string model, int ozyGB, int cameraMPx, SmartphoneType type)
    {
        Brand = brand;
        Model = model;
        OzyGB = ozyGB;
        CameraMPx = cameraMPx;
        Type = type;
    }

    public void ShowInfo()
    {
        Console.WriteLine($"Бренд: {Brand}");
        Console.WriteLine($"Модель: {Model}");
        Console.WriteLine($"ОЗУ: {OzyGB} ГБ");
        Console.WriteLine($"Камера: {CameraMPx} Мп");
        Console.WriteLine($"Потужність: {Type}");
        Console.WriteLine(new string('-', 25));
    }

    public void Call() => Console.WriteLine($"{Brand} {Model} може дзвонити");
    public void Photo() => Console.WriteLine($"{Brand} {Model} може фотографувати");
    public void Internet() => Console.WriteLine($"{Brand} {Model} може виходити в інтернет");
    public void Temperature() => Console.WriteLine($"{Brand} {Model} може нагріватися");

    public void UpgradeRAM(int extraGB)
    {
        if (extraGB > 0 && OzyGB + extraGB <= 512)
        {
            OzyGB += extraGB;
            Console.WriteLine($"ОЗУ збільшено! Тепер {OzyGB} ГБ");
        }
        else
            Console.WriteLine("Неможливо збільшити ОЗУ!");
    }

    public void CompareCamera(Smart other)
    {
        if (this.CameraMPx > other.CameraMPx)
            Console.WriteLine($"{this.Brand} {this.Model} має кращу камеру ({this.CameraMPx} Мп) ніж {other.Brand} {other.Model} ({other.CameraMPx} Мп).");
        else if (this.CameraMPx < other.CameraMPx)
            Console.WriteLine($"{other.Brand} {other.Model} має кращу камеру ({other.CameraMPx} Мп) ніж {this.Brand} {this.Model} ({this.CameraMPx} Мп).");
        else
            Console.WriteLine("У обох смартфонів однакова камера.");
    }
}
