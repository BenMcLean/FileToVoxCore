namespace FileToVoxCoreTest
{
	public class UnitTest1
	{
		public const string InputPath = @"..\..\..\Input.vox",
			OutputPath = @"..\..\..\Output.vox";
		[Fact]
		public void Test1()
		{
			Assert.True(File.Exists(InputPath));
			if (File.Exists(OutputPath))
				File.Delete(OutputPath);
			FileToVoxCore.Vox.VoxModel voxModel = new FileToVoxCore.Vox.VoxReader().LoadModel(InputPath);
			FileToVoxCore.Vox.VoxelData voxelData = voxModel.VoxelFrames[0];
			static ulong Encode(ushort x, ushort y, ushort z) => ((ulong)z << 32) | ((uint)y << 16) | x;
			//static void Decode(ulong @ulong, out ushort x, out ushort y, out ushort z)
			//{
			//	x = (ushort)@ulong;
			//	y = (ushort)(@ulong >> 16);
			//	z = (ushort)(@ulong >> 32);
			//}
			Dictionary<ulong, byte> dictionary = new();
			ushort sizeX = (ushort)(voxelData.VoxelsWide - 1),
				sizeY = (ushort)(voxelData.VoxelsTall - 1),
				sizeZ = (ushort)(voxelData.VoxelsDeep - 1);
			for (ushort x = 0; x < sizeX; x++)
				for (ushort y = 0; y < sizeY; y++)
					for (ushort z = 0; z < sizeZ; z++)
						if (voxelData.GetSafe(x, y, z) is byte voxel && voxel != 0)
							dictionary.Add(Encode(x, y, z), voxel);
			new FileToVoxCore.Vox.VoxWriter().WriteModel(
				absolutePath: OutputPath,
				palette: voxModel.Palette.ToList(),
				schematic: new FileToVoxCore.Schematics.Schematic(dictionary
					.Select(voxel => new FileToVoxCore.Schematics.Voxel(
						x: (ushort)voxel.Key,
						y: (ushort)(voxel.Key >> 16),
						z: (ushort)(voxel.Key >> 32),
						color: (uint)voxModel.Palette[voxel.Value].ToArgb()))
					.ToList()));
			Assert.True(File.Exists(OutputPath));
		}
	}
}
