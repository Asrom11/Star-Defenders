using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StarDefenderss;

public interface IEnemy: IObject
{
    Grid _grid { get; set; }
    void Update(GameTime gameTime);

    static List<Node> _path;
}