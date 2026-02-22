using System.IO;
using NUnit.Framework;

namespace TileRift.Tests.EditMode
{
    public sealed class StoreContentTests
    {
        [Test]
        public void StoreFiles_Exist()
        {
            Assert.That(File.Exists("Store/store_assets_checklist.md"), Is.True);
            Assert.That(File.Exists("Store/store_listing_tr.md"), Is.True);
            Assert.That(File.Exists("Store/store_listing_en.md"), Is.True);
        }

        [Test]
        public void StoreListings_HaveMinimumSections()
        {
            var tr = File.ReadAllText("Store/store_listing_tr.md");
            var en = File.ReadAllText("Store/store_listing_en.md");

            Assert.That(tr.Contains("## Baslik"), Is.True);
            Assert.That(tr.Contains("## Kısa Açıklama"), Is.True);
            Assert.That(tr.Contains("## Uzun Açıklama"), Is.True);
            Assert.That(tr.Contains("## Anahtar Kelimeler"), Is.True);

            Assert.That(en.Contains("## Title"), Is.True);
            Assert.That(en.Contains("## Short Description"), Is.True);
            Assert.That(en.Contains("## Long Description"), Is.True);
            Assert.That(en.Contains("## Keywords"), Is.True);
        }
    }
}
