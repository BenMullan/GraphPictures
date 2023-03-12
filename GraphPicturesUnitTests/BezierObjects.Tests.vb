Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports GraphPictures.Library

'''<summary>
'''This is a test class for BezierObjectsTests and is intended
'''to contain all BezierObjectsTests Unit Tests
'''</summary>
<TestClass()> _
Public Class BezierObjectsTests


	Private testContextInstance As TestContext

	'''<summary>
	'''Gets or sets the test context which provides
	'''information about and functionality for the current test run.
	'''</summary>
	Public Property TestContext() As TestContext
		Get
			Return testContextInstance
		End Get
		Set(ByVal value As TestContext)
			testContextInstance = Value
		End Set
	End Property

#Region "Additional test attributes"
	'
	'You can use the following additional attributes as you write your tests:
	'
	'Use ClassInitialize to run code before running the first test in the class
	'<ClassInitialize()>  _
	'Public Shared Sub MyClassInitialize(ByVal testContext As TestContext)
	'End Sub
	'
	'Use ClassCleanup to run code after all tests in a class have run
	'<ClassCleanup()>  _
	'Public Shared Sub MyClassCleanup()
	'End Sub
	'
	'Use TestInitialize to run code before running each test
	'<TestInitialize()>  _
	'Public Sub MyTestInitialize()
	'End Sub
	'
	'Use TestCleanup to run code after each test has run
	'<TestCleanup()>  _
	'Public Sub MyTestCleanup()
	'End Sub
	'
#End Region


	'''<summary>
	'''A test for WithAllPointsIncremented
	'''</summary>
	<TestMethod(), _
	 DeploymentItem("GraphPictures.Library.dll")> _
	Public Sub WithAllPointsIncrementedTest()
		Dim _Expression As String = String.Empty ' TODO: Initialize to an appropriate value
		Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
		Dim actual As String
		actual = GraphPictures.Library.BezierObjects_Accessor.WithAllPointsIncremented(_Expression)
		Assert.AreEqual(expected, actual)
		Assert.Inconclusive("Verify the correctness of this test method.")
	End Sub
End Class