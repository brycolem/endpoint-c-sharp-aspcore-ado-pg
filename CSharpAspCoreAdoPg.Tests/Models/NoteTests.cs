using Xunit;
using CSharpAspCoreAdoPg.Models;

namespace CSharpAspCoreAdoPg.Tests.Models
{
  public class NoteTests
  {
    [Fact]
    public void Note_Should_Initialize_With_ExpectedValues()
    {
      var note = new Note
      {
        Id = 1,
        NoteText = "This is a test note",
        ApplicationId = 101
      };

      Assert.Equal(1, note.Id);
      Assert.Equal("This is a test note", note.NoteText);
      Assert.Equal(101, note.ApplicationId);
    }

    [Fact]
    public void Note_DefaultValues_ShouldBe_NullOrZero()
    {
      var note = new Note();

      Assert.Equal(0, note.Id);
      Assert.Equal(string.Empty, note.NoteText);
      Assert.Equal(0, note.ApplicationId);
    }
  }
}
