namespace mxd.DukeBuilder.Types
{
	[TypeHandler(PropertyType.SoundRadius, "Sound Radius")]
	internal class SoundRadiusHandler : IntegerHandler
	{
		public override int MinValue { get { return 0; } }
	}
}
