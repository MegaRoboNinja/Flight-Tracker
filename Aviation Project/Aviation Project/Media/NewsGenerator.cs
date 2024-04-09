namespace Aviation_Project;

public class NewsGenerator
{
    private List<Media> media;
    private List<IReportable> reportables;
    private int _mediaIt = 0;
    private int _reportableIt = 0;
    public NewsGenerator(List<Media> media, List<IReportable> reportables)
    {
        this.media = media;
        this.reportables = reportables;
    }

    public string GenerateNextNews()
    {
        // take next pair from the cartesian product of the media and reportables lists
        // get the news report about the reportable object from that media
        // return the news report or null if there are no more pairs
        if(_reportableIt >= reportables.Count || _mediaIt >= media.Count)
            return null;
        
        Media medium = media[_mediaIt];
        IReportable reportable = reportables[_reportableIt];
        string news = reportable.acceptReport(medium);
        
        // Move the iterator logic:
        if (_mediaIt < media.Count - 1)
            _mediaIt++;
        else
        {
            _mediaIt = 0;
            _reportableIt++;
        }

        return news;
    }
    
}