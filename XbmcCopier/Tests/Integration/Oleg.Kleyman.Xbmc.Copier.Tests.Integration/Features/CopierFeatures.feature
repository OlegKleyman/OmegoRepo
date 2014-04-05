Feature: CopierFeatures
	When video media torrents download
	they need to be placed in the correct directory

Scenario: When a TV torrent downloads it needs to be copied to the correct directory
	Given a TV torrent has finished downloading
	When the XbmcFilerCopier runs
	Then the torrent should be downloaded to the .\Work\TV directory
