using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDanger
{
    void ChangeDangerState(Tile tile, int spriteNum);
    void DestroyPlayerOrNot();
    // ������� ��������� ������ ������� ��������, ������ ������� ����� ��������������� ��������������� ������ � ���������� �������
}