using NUnit.Framework;
using TileRift.Core;
using TileRift.Runtime;

namespace TileRift.Tests.EditMode
{
    public sealed class BoardTileGlyphTests
    {
        [Test]
        public void ToLabel_MapsKnownTiles()
        {
            Assert.That(BoardTileGlyph.ToLabel(TileType.Red), Is.EqualTo("R"));
            Assert.That(BoardTileGlyph.ToLabel(TileType.Green), Is.EqualTo("G"));
            Assert.That(BoardTileGlyph.ToLabel(TileType.Blue), Is.EqualTo("B"));
            Assert.That(BoardTileGlyph.ToLabel(TileType.Yellow), Is.EqualTo("Y"));
            Assert.That(BoardTileGlyph.ToLabel(TileType.None), Is.EqualTo("."));
        }
    }
}
