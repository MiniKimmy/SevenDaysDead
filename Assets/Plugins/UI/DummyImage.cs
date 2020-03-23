using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DummyImage : Image {

	protected override void OnPopulateMesh(VertexHelper toFill) {
		toFill.Clear();
	}
}
