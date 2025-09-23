using System;

public class Smart
{
    private string brand;
    private string model;
    private int ozyGB;
    private int cameraMPx;

    public int BatteryLevel { get; set; } = 100;

    public string Brand
    {
        get => brand;
        set
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length < 3 || value.Length > 12)
                throw new ArgumentException("Бренд має бути від 3 до 12 символів!");
            brand = value;
        }
    }

    public string Model
    {
        get => model;
        set
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length < 2 || value.Length > 12)
                throw new ArgumentException("Модель має бути від 2 до 12 символів!");
            model = value;
        }
    }

    public int OzyGB
    {
        get => ozyGB;
        set
        {
            if (value < 1 || value > 512)
                throw new ArgumentException("ОЗУ повинно бути від 1 до 512 ГБ!");
            ozyGB = value;
            UpdateType();
        }
    }

    public int CameraMPx
    {
        get => cameraMPx;
        set
        {
            if (value < 1 || value > 264)
                throw new ArgumentException("Камера повинна бути від 1 до 264 Мп!");
            cameraMPx = value;
        }
    }

    public SmartphoneType Type { get; private set; }

    public Smart()
    {
        Brand = "Невідомо";
        Model = "Невідомо";
        OzyGB = 1;
        CameraMPx = 1;
        Type = SmartphoneType.Weak;
    }

    public Smart(string brand, string model, int ozyGB)
        : this(brand, model, ozyGB, 1, GetTypeByRam(ozyGB))
    {
    }

    public Smart(string brand, string model, int ozyGB, int cameraMPx, SmartphoneType type)
    {
        Brand = brand;
        Model = model;
        OzyGB = ozyGB;
        CameraMPx = cameraMPx;
        Type = type;
    }

    public string GetInfo()
    {
        return $"Бренд: {Brand}\nМодель: {Model}\nОЗУ: {OzyGB} ГБ\nКамера: {CameraMPx} Мп\nПотужність: {Type}\nБатарея: {BatteryLevel}%\n{new string('-', 25)}";
    }

    public string Call() => $"{Brand} {Model} може дзвонити";

    public string Call(string number) => $"{Brand} {Model} телефонує на номер {number}";

    public string Photo() => $"{Brand} {Model} може фотографувати";
    public string Internet() => $"{Brand} {Model} може виходити в інтернет";
    public string Temperature() => $"{Brand} {Model} може нагріватися";

    public string UpgradeRAM(int extraGB)
    {
        if (extraGB > 0 && OzyGB + extraGB <= 512)
        {
            OzyGB += extraGB;
            return $"ОЗУ збільшено! Тепер {OzyGB} ГБ";
        }
        return "Неможливо збільшити ОЗУ!";
    }

    public string CompareCamera(Smart other)
    {
        if (this.CameraMPx > other.CameraMPx)
            return $"{this.Brand} {this.Model} має кращу камеру ({this.CameraMPx} Мп) ніж {other.Brand} {other.Model} ({other.CameraMPx} Мп).";
        else if (this.CameraMPx < other.CameraMPx)
            return $"{other.Brand} {other.Model} має кращу камеру ({other.CameraMPx} Мп) ніж {this.Brand} {this.Model} ({this.CameraMPx} Мп).";
        else
            return "У обох смартфонів однакова камера.";
    }

    private void UpdateType()
    {
        if (OzyGB <= 4)
            Type = SmartphoneType.Weak;
        else if (OzyGB <= 12)
            Type = SmartphoneType.Average;
        else
            Type = SmartphoneType.Powerful;
    }

    private static SmartphoneType GetTypeByRam(int ozyGB)
    {
        if (ozyGB <= 4) return SmartphoneType.Weak;
        if (ozyGB <= 12) return SmartphoneType.Average;
        return SmartphoneType.Powerful;
    }
}
