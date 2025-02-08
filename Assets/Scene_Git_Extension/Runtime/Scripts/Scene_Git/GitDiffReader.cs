using System.Collections.Generic;
using UnityEngine;

public interface GitDiffReader
{
   public IList<GameObject> GetDiffObjects();

}