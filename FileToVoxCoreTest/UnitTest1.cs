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
			List<FileToVoxCore.Schematics.Voxel> voxels = new();
			ushort sizeX = (ushort)(voxelData.VoxelsWide - 1),
				sizeY = (ushort)(voxelData.VoxelsTall - 1),
				sizeZ = (ushort)(voxelData.VoxelsDeep - 1);
			for (ushort x = 0; x < sizeX; x++)
				for (ushort y = 0; y < sizeY; y++)
					for (ushort z = 0; z < sizeZ; z++)
						if (voxelData.GetSafe(x, y, z) is byte voxel && voxel != 0)
							voxels.Add(new FileToVoxCore.Schematics.Voxel(
								x: x,
								y: y,
								z: z,
								color: (uint)voxModel.Palette[voxel].ToArgb()));
			new FileToVoxCore.Vox.VoxWriter().WriteModel(
				absolutePath: OutputPath,
				palette: voxModel.Palette.ToList(),
				schematic: new FileToVoxCore.Schematics.Schematic(voxels));
			Assert.True(File.Exists(OutputPath));
		}
	}
}
