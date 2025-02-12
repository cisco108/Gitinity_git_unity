using System.Collections.Generic;
using UnityEngine;

public interface IGitDiffReader
{
   public IList<GameObject> GetDiffObjects();

}