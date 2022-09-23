using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Game : MonoBehaviour
{
    public Texture[] textures;
    public Cell[] cubePrefabs;
    int numLayers = 20;
    int numRows = 10;
    int numCols = 10;
	Cell[,,] data;//layer,x,y
    int count;
    public Transform bar;
    bool fail=false;

    List<Cell> cellsOnBar;
    public GameUI ui;

    bool downExistBox(int layer,int row,int col)
    {
		if (layer == 0 )
		{
			return true;
		}
		for (var i = -1; i <= 1; i++)
		{
			for (var j = -1; j <= 1; j++)
			{
				var l = layer - 1;
				var r = row + i;
				var c = col + j;


				if(r >= 0 && c >= 0 && r < numRows && c < numCols && data[l,r,c]!=null)
                {
                    return true;
                }
			}
		}
        return false;
	}

    public void ReStart()
    {
        //DOTween.KillAll();
        count = 0;
        fail = false;
        if(cellsOnBar!=null)
        foreach (Cell cell in cellsOnBar)
        {
            cell.transform.DOKill();
            Destroy(cell.gameObject);
        }

        cellsOnBar = new List<Cell>();
        if (data != null)
        {
            for (var i = 0; i < numLayers; i++)
            {
                for (var j = 0; j < numRows; j++)
                {
                    for (var k = 0; k < numCols; k++)
                    {
                        var cell = data[i, j, k];
                        if (cell != null)
                        {
                            cell.transform.DOKill();
                            Destroy(cell.gameObject);
                        }
                    }
                }
            }
        }
		data = new Cell[numLayers, numRows, numCols];
		for (var i = 0; i < numLayers; i++)
		{
			for (var j = 0; j < numRows; j++)
			{
				for (var k = 0; k < numCols; k++)
				{
					if (Random.value < 0.5f && i % 2 == j % 2 && i % 2 == k % 2&&downExistBox(i,j,k))
					{
						var size = 1;
						var sizeL = 1f;
						var cube = Instantiate(cubePrefabs[0].gameObject, new Vector3(((float)k - numCols / 2) * size, i * sizeL, ((float)j - numRows / 2) * size), Quaternion.identity, gameObject.transform);
						count++;
						var cell = cube.GetComponent<Cell>();
						var v = Random.Range(0, textures.Length);
						cell.texture = textures[v];
						cell.Value = v;

						cell.layer = i;
						cell.row = j;
						cell.col = k;
						cell.OnClick.AddListener(() => {
							if (fail)
							{
								return;
							}

							OneShotAudio.playOneShot("166384774385063");
							print("onclick" + cell.layer + "," + cell.row + "," + cell.col);
							data[cell.layer, cell.row, cell.col] = null;
                            updateAllCell();
                            //updateCell(cell.layer, cell.row, cell.col, true);
							addCellToBar(cell);

							count--;
							if (count <= 0 && !fail)
							{
								print("Í¨¹Ø");
								ui.Show(true, false);
							}
						});
						data[i, j, k] = cell;
					}

				}
			}
		}
        updateAllCell();
	}

    void updateAllCell()
    {
        for (var i = 0; i < numLayers; i++)
		{
			for (var j = 0; j < numRows; j++)
			{
				for (var k = 0; k < numCols; k++)
				{
					var v = data[i, j, k];
					if (v != null)
					{
						updateCell(i, j, k);
					}
				}
			}
		}
    }

    void Start()
    {
		ui.Show(false, false);
	}

    void updateCellOnBar()
    {
        print(cellsOnBar.Count);
        for (var i = 0; i < cellsOnBar.Count; i++)
        {
            var c = cellsOnBar[i];
            c.transform.DOLocalMoveX(i * 2+1,.5f);
        }
    }

    void addCellToBar(Cell cell)
    {
        var added = false;
        cell.transform.localEulerAngles = new Vector3(-90, 0, 0);
        cell.transform.localPosition = new Vector3(8 * 2 + 1, 0, 0);
        for (var i = 0; i < cellsOnBar.Count; i++) {
            var c = cellsOnBar[i];
            if (c.Value==cell.Value)
            {
                if (i<cellsOnBar.Count-1)
                {
                    if (cellsOnBar[i+1].Value==cell.Value)
                    {
                        var a = cellsOnBar[i].gameObject;
                        var b = cellsOnBar[i + 1].gameObject;
						cellsOnBar.RemoveRange(i, 2);
                        var ce = cell.gameObject;
						ce.transform.DOLocalMoveX(b.transform.localPosition.x+2, .5f).onComplete=() => {
                            Destroy(a);
                            Destroy(b);
                            Destroy(ce);
						    OneShotAudio.playOneShot("166384774687269");
                        };
					}
                    else
                    {
                        cellsOnBar.Insert(i+1, cell);
                    }
                    added = true;
                }
                break;
            }
        }
        if (!added) {
            cellsOnBar.Add(cell);
        }
        cell.transform.SetParent(bar.transform,false);
        cell.mouseEnabled = false;
        cell.setAlpha(false);

        updateCellOnBar();
        if (cellsOnBar.Count>=7)
        {
            print("Ê§°Ü");
			ui.Show(false, true);
			fail = true;
        }
    }

    private void updateCell(int layer,int row,int col)
    {
        if (layer<0||!(row >= 0 && col >= 0 && row < numRows && col < numCols))
        {
            return;
        }

        var e = true;
        if (layer<numLayers-1)
        {
            for (var i = -1; i <= 1; i++) { 
                for (var j = -1; j <= 1; j++)
                {
                    var l = layer + 1;
                    var r = row + i;
                    var c= col + j;
                    if (r>=0&&c>=0&&r<numRows&&c<numCols&&data[l,r,c]!=null)
                    {
                        e = false; break;
                    }
                }
                if (!e)
                {
                    break;
                }
            }
        }

		var obj = data[layer, row, col];
        if (obj != null)
        {
            obj.mouseEnabled = e;
        }
        /*else if(updateDown)
        {
            for (var i = -1; i <= 1; i++)
            {
                for (var j = -1; j <= 1; j++)
                {
                    var l = layer - 1;
                    var r = row + i;
                    var c = col + j;
                    updateCell(l, r, c, true);
                }
            }
        }*/
    }
}
