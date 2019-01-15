namespace Assets.Scripts.Json
{
    public struct TheoryListTexts
    {
        public string Title;
        public string Text;
    }

    public struct TheoryListImages
    {
        public string Image;
    }

    public struct TheoryListVideos
    {
        public string Thumbnail;
        public string Url;
    }

    public class TheoryList
	{
		public TheoryListTexts[] TheoryListTexts;
	    public TheoryListImages[] TheoryListImages;
	    public TheoryListVideos[] TheoryListVideos;

		public TheoryList()
		{

		}

		public TheoryList(TheoryListTexts[] texts, TheoryListImages[] images, TheoryListVideos[] thumbnails)
		{
		    TheoryListTexts = texts;
		    TheoryListImages = images;
		    TheoryListVideos = thumbnails;
		}
	}
}