namespace WebApiContrib.Formatting.Siren.Client
{
    public static class SirenExtensions
    {
        public static bool IsLink(this IEmbeddedEntity This)
        {
            return This is IEmbeddedLink;
        }

        public static IEmbeddedLink AsLink(this IEmbeddedEntity This)
        {
            return This as IEmbeddedLink;
        }

        public static bool IsRepresentation(this IEmbeddedEntity This)
        {
            return This is IEmbeddedRepresentation;
        }

        public static IEmbeddedRepresentation AsRepresentation(this IEmbeddedEntity This)
        {
            return This as IEmbeddedRepresentation;
        }
    }
}
