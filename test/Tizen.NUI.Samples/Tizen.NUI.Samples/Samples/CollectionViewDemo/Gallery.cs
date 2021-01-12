using System.Collections.Generic;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;
using Tizen.NUI.Binding;

class Gallery
{
    string sourceDir = Tizen.NUI.Samples.CommonResource.GetDaliResourcePath()+"ItemViewDemo/gallery/gallery-medium-";
    public string Name { get; set; }
    private int _index;
    public Gallery(int index, string name)
    {
        _index = index;
        Name = name;
    }
    public string ViewLabel
    {
        get
        {
            return "[" + _index + "] : " + Name;
        }
    }

    public string ImageUrl
    {
        get
        {
            return sourceDir+(_index%20)+".jpg";
        }
    }

}
class GalleryViewModel
{
    string[] namePool = {
        "Cat",
        "Boy",
        "Arm muscle",
        "Girl",
        "House",
        "Cafe",
        "Statue",
        "Sea",
        "hosepipe",
        "Police",
        "Rainbow",
        "Icicle",
        "Tower with the Moon",
        "Giraffe",
        "Camel",
        "Zebra",
        "Red Light",
        "Banana",
        "Lion",
        "Espresso",
    };
    public GalleryViewModel() {}
    public List<Gallery> CreateData(int count)
    {
        List<Gallery> result = new List<Gallery>();
        for (int i = 0; i < count; i++)
        {
            result.Add(new Gallery(i, namePool[i%20]));
        }
        return result;
    }
}