namespace PrintHub.Helpers;

public static class EventTypeConstants
{
    public const string STARTED = "Started";
    public const string COMPLETED = "Completed";
    public const string FAILED = "Failed";

    public static readonly string[] ALL = new[]
    {
        STARTED,
        COMPLETED,
        FAILED
    };
}

public static class FilamentMaterialConstants
{
    public const string ABS = "ABS";
    public const string ABS_PLUS = "ABS+";
    public const string ASA = "ASA";

    public const string HIPS = "HIPS";

    public const string NYLON = "Nylon";
    public const string NYLON_CF = "Nylon-CF";
    public const string NYLON_GF = "Nylon-GF";

    public const string PC = "PC";
    public const string PC_ABS = "PC-ABS";

    public const string PETG = "PETG";
    public const string PETG_CF = "PETG-CF";
    public const string PETG_GF = "PETG-GF";

    public const string PLA = "PLA";
    public const string PLA_PLUS = "PLA+";
    public const string PLA_TOUGH = "PLA-Tough";
    public const string PLA_CF = "PLA-CF";
    public const string PLA_GF = "PLA-GF";
    public const string PLA_WOOD = "PLA-Wood";
    public const string PLA_METAL = "PLA-Metal";

    public const string PP = "PP";

    public const string PVA = "PVA";

    public const string TPU = "TPU";
    public const string TPE = "TPE";

    public const string WOOD = "Wood";
    public const string METAL = "Metal";

    public static readonly string[] ALL = new[]
    {
        ABS,
        ABS_PLUS,
        ASA,
        HIPS,
        NYLON,
        NYLON_CF,
        NYLON_GF,
        PC,
        PC_ABS,
        PETG,
        PETG_CF,
        PETG_GF,
        PLA,
        PLA_PLUS,
        PLA_TOUGH,
        PLA_CF,
        PLA_GF,
        PLA_WOOD,
        PLA_METAL,
        PP,
        PVA,
        TPU,
        TPE,
        WOOD,
        METAL
    };
}

public static class FilamentTextureConstants
{
    public const string BASIC = "Basic";
    public const string MATTE = "Matte";
    public const string SILK = "Silk";
    public const string GLOSSY = "Glossy";
    public const string TRANSLUCENT = "Translucent";
    public const string TRANSPARENT = "Transparent";
    public const string METALLIC = "Metallic";
    public const string WOOD = "Wood";

    public static readonly string[] ALL = new[]
    {
        BASIC,
        MATTE,
        SILK,
        GLOSSY,
        TRANSLUCENT,
        TRANSPARENT,
        METALLIC,
        WOOD
    };
}

public static class FilamentWeightConstants
{
    // TODO: should this be an enum so it can grab the text ("250 grams") and the value (.250m) together?
    public const decimal GRAMS_250 = .250m; // 250 grams
    public const decimal GRAMS_500 = .500m; // 500 grams
    public const decimal GRAMS_750 = .750m; // 750 grams
    public const decimal KILOGRAM_1 = 1m;   // 1 kilogram
    public const decimal KILOGRAM_2 = 2m;   // 2 kilogram

    public const decimal DEFAULT = KILOGRAM_1;

    public static readonly decimal[] ALL = new[]
    {
        GRAMS_250,
        GRAMS_500,
        GRAMS_750,
        KILOGRAM_1,
        KILOGRAM_2
    };
}

public static class MaterialUnitConstants
{
    public const string EACH = "Each";
    public const string GRAMS = "Grams";
    public const string MILLILITERS = "Milliliters";
    public const string INCHES = "Inches";
    public const string FEET = "Feet";

    public static readonly string[] ALL = new[]
    {
        EACH,
        GRAMS,
        MILLILITERS,
        INCHES,
        FEET
    };
}

public static class PrinterTypeConstants
{
    public const string TYPE_3D = "3D";
    public const string TYPE_LASER = "Laser";
    public const string TYPE_CUTTER = "Cutter";

    public static readonly string[] ALL = new[]
    {
        TYPE_3D,
        TYPE_LASER,
        TYPE_CUTTER
    };
}

public static class SystemUsersConstants
{
    public const string SYSTEM_USER = "System";
}
