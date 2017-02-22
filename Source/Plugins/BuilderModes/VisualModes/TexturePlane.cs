using mxd.DukeBuilder.Geometry;

namespace mxd.DukeBuilder.EditModes.VisualModes
{
	internal struct TexturePlane
	{
		// Geometry coordinates (left-top, right-top and right-bottom)
		public Vector3D VertTopLeft;
		public Vector3D VertTopRight;
		public Vector3D VertBottomRight;

		// Texture coordinates on the points above
		public Vector2D TextureTopLeft;
		public Vector2D TextureTopRight;
		public Vector2D TextureBottomRight;

		// This returns interpolated texture coordinates for the point p on the plane defined by vlt, vrt and vrb
		public Vector2D GetTextureCoordsAt(Vector3D p)
		{
			// Delta vectors
			Vector3D v31 = VertBottomRight - VertTopLeft;
			Vector3D v21 = VertTopRight - VertTopLeft;
			Vector3D vp1 = p - VertTopLeft;

			// Compute dot products
			float d00 = Vector3D.DotProduct(v31, v31);
			float d01 = Vector3D.DotProduct(v31, v21);
			float d02 = Vector3D.DotProduct(v31, vp1);
			float d11 = Vector3D.DotProduct(v21, v21);
			float d12 = Vector3D.DotProduct(v21, vp1);

			// Compute barycentric coordinates
			float invd = 1.0f / (d00 * d11 - d01 * d01);
			float u = (d11 * d02 - d01 * d12) * invd;
			float v = (d00 * d12 - d01 * d02) * invd;

			// Delta texture coordinates
			Vector2D t21 = TextureTopRight - TextureTopLeft;
			Vector2D t31 = TextureBottomRight - TextureTopLeft;

			// Lerp
			return TextureTopLeft + t31 * u + t21 * v;
		}
	}
}
