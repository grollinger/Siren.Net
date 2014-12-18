namespace WebApiContrib.Formatting.Siren.Client
{
    public enum FieldType
    {
        Text, // Default -> First
        Hidden,
        Search,
        Tel,
        Url,
        Email,
        Password,
        Datetime,
        Date,
        Month,
        Week,
        Time,

        //DatetimeLocal, // needs special handling during deserialization
        Number,

        Range,
        Color,
        Checkbox,
        Radio,
        File,
        Image,
        Button
    }
}