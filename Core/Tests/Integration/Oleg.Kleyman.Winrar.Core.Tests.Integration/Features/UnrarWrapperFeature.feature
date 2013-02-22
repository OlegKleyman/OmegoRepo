Feature: UnrarWrapper
	As a Developer I need to unit test
	code and need a wrapper class for the
	unrar interop to do it easily.

Scenario: Should be able to Open and close archive
	Given I have an instance of the NativeMethods object	
	And I instantiate an UnrarWrapper object
	When I call the the Open method with ..\..\..\..\..\..\Common\Test\Oleg.Kleyman.Winrar.Core.Tests.Integration\TestFolder.rar archive path
	Then I should receive a greater than 0 IntPtr handle back
	When I call the Close method
	Then It should return a success value back

Scenario: Should return files in archive
	Given I have an instance of the NativeMethods object
	And I instantiate an UnrarWrapper object
	When I call the the Open method with ..\..\..\..\..\..\Common\Test\Oleg.Kleyman.Winrar.Core.Tests.Integration\TestFolder.rar archive path
	And I call the GetFiles method
	Then I should get the following list back
	| HighFlags          | UnpackedSize | PackedSize | LastModificationDate    | Name                       | Volume                                                                                        | LowFlags |
	| DictionarySize512K | 0            | 0          | 2012-08-04 09:15:34.000 | TestFolder\testFile.txt    | C:\GitRepos\MainDefault\Common\Test\Oleg.Kleyman.Winrar.Core.Tests.Integration\TestFolder.rar | None     |
	| DictionarySize512K | 297541       | 41         | 2012-06-13 22:00:58.000 | test.txt                   | C:\GitRepos\MainDefault\Common\Test\Oleg.Kleyman.Winrar.Core.Tests.Integration\TestFolder.rar | None     |
	| DirectoryRecord    | 0            | 0          | 2012-08-04 09:15:22.000 | TestFolder\InnerTestFolder | C:\GitRepos\MainDefault\Common\Test\Oleg.Kleyman.Winrar.Core.Tests.Integration\TestFolder.rar | None     |
	| DirectoryRecord    | 0            | 0          | 2012-08-04 09:15:38.000 | TestFolder                 | C:\GitRepos\MainDefault\Common\Test\Oleg.Kleyman.Winrar.Core.Tests.Integration\TestFolder.rar | None     |