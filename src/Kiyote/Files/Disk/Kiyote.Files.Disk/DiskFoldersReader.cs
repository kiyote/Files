﻿namespace Kiyote.Files.Disk;

public sealed class DiskFoldersReader : IFoldersReader {

	private readonly IFileSystem _fileSystem;
	private readonly DiskFileSystemConfiguration _config;
	private readonly FolderId _root;

	public DiskFoldersReader(
		DiskFileSystemConfiguration config
	) : this( new FileSystem(), config ) {
	}

	public DiskFoldersReader(
		IFileSystem fileSystem,
		DiskFileSystemConfiguration config
	) {
		ArgumentNullException.ThrowIfNull( fileSystem );
		ArgumentNullException.ThrowIfNull( config );
		_fileSystem = fileSystem;
		_config = config;
		_root = new FolderId( _config.FileSystemId, "\\" );
	}

	FolderId IFoldersReader.Root => _root;

	IEnumerable<FileId> IFoldersReader.GetFilesInFolder(
		FolderId folderId
	) {
		_config.EnsureValidId( folderId );
		string path = _config.ToPath( folderId );
		foreach( string fileName in _fileSystem.Directory.EnumerateFiles( path ) ) {
			yield return _config.ToFileId( folderId, fileName );
		}
	}

	IEnumerable<FolderId> IFoldersReader.GetFoldersInFolder(
		FolderId folderId
	) {
		_config.EnsureValidId( folderId );
		string path = _config.ToPath( folderId );
		foreach( string folderName in _fileSystem.Directory.EnumerateDirectories( path ) ) {
			yield return _config.ToFolderId( folderId, folderName );
		}
	}
}
