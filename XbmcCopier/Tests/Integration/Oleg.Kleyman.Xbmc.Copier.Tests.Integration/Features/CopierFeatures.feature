Feature: CopierFeatures
	When video media torrents download
	they need to be placed in the correct directory

Scenario: When a TV torrent downloads it needs to be copied to the correct directory
	Given a Tv torrent has finished downloading
	When the XbmcFilerCopier runs
	Then the torrent contents should be transfered to the correct directory
