using AampLibrary;
using Revrs;

RevrsReader reader = new(File.ReadAllBytes(
    @"D:\bin\AampLibrary\Armor_006_Upper.bphysics"
));
ImmutableAamp aampView = new(ref reader);

Aamp aamp = Aamp.FromImmutable(ref aampView);
// write binary
