using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using UnityEngine;

[StructLayout(LayoutKind.Sequential)]
public struct MeshInfo
{
    public int meshId, numVertices, numFaceIndices;
}

public struct Matrix4
{
    public float	m00, m01, m02, m03,
        m10, m11, m12, m13,
        m20, m21, m22, m23,
        m30, m31, m32, m33;
};

public class InfiniTAMConnector : MonoBehaviour
{
    public GameObject prefab;
    public GameObject parent;

    public static bool sendTransformToCam = true;
    
    private static Dictionary<int, GameObject> activeMeshes;
    
    // we have two buffers in InfiniTAM Client
    private const int NUMBUFFERS = 2;
    private int currentBufferNumber = 0;
    private SharedMeshData[] sharedMeshBuffers = new SharedMeshData[NUMBUFFERS];
    
    private const string meshInfoMutexName = "MESHINFO_MUTEX_";
    private const string meshInfoFileName = "MESHINFO_SHAREDMEMORY_";
    
    private const string verticesMutexName = "VERTICES_MUTEX_";
    private const string verticesFileName = "VERTICES_SHAREDMEMORY_";
    
    private const string normalsMutexName = "NORMALS_MUTEX_";
    private const string normalsFileName = "NORMALS_SHAREDMEMORY_";
    
    private const string facesMutexName = "FACES_MUTEX_";
    private const string facesFileName = "FACES_SHAREDMEMORY_";
    
    private const string colorsMutexName = "COLORS_MUTEX_";
    private const string colorsFileName = "COLORS_SHAREDMEMORY_";
    
    private const string cameraPosMutexName = "CAMERA_POS_MUTEX";
    private const string cameraPosFileName = "CAMERA_POS_SHAREDMEMORY";

    private SharedMemoryAccess cameraPosSharedMemory;

    private bool sharedMemoryInitialized = false;
    private bool clientReady = false;

    // Start is called before the first frame update
    void Start()
    {
        if (activeMeshes == null)
        {
            activeMeshes = new Dictionary<int, GameObject>();
        }
        
        try
        {
            //cameraPosSharedMemory = new SharedMemoryAccess(cameraPosMutexName, cameraPosFileName);
            sharedMeshBuffers[0] = new SharedMeshData(0);
            sharedMeshBuffers[1] = new SharedMeshData(1);
            sharedMemoryInitialized = true;
        }
        catch (Exception e)
        {
            UIManager.WriteToLogger("Shared Memory not found with " + e.GetType());
            Debug.Log("Client not found." + e.GetType());
            return;
        }

        // Wait for shared memory allocation of client
        UIManager.WriteToLogger("Client found. Wait for green light.");
        Debug.Log("Client found. Wait for shared memory allocation.");
    }

    // Update is called once per frame
    void Update()
    {
        if (sharedMemoryInitialized)
        {
            ReadSharedMemory();    
        }
    }

    Color[] CreateColorsFromVector4Array(Vector4[] colorsAsVectorArray)
    {
        Color[] result = new Color[colorsAsVectorArray.Length];

        for (int i = 0; i < colorsAsVectorArray.Length; i++)
        {
            result[i] = new Color(colorsAsVectorArray[i].x, colorsAsVectorArray[i].y, colorsAsVectorArray[i].z, colorsAsVectorArray[i].w);
        }
        
        return result;
    }

    public void UpdateMesh(GameObject mesh, Vector3[] newVertices, Vector3[] normals, int[] faceIndices, Vector4[] colorsAsVectors)
    {
        Color[] colors = CreateColorsFromVector4Array(colorsAsVectors);
        
        Mesh newMesh = new Mesh();
        newMesh.vertices = newVertices;
        newMesh.normals = normals;
        newMesh.triangles = faceIndices;
        newMesh.colors = colors;

        mesh.GetComponent<MeshFilter>().mesh = newMesh;
    }
    
    public void ReadSharedMemory()
    {
        /*
        Matrix4 cameraPosTemp;
        
        cameraPosSharedMemory.Lock();
        cameraPosSharedMemory.accessor.Read<Matrix4>(0, out cameraPosTemp);
        cameraPosSharedMemory.Unlock();

        Matrix4x4 cameraPos;
        
        #region MatrixCopy
        cameraPos.m00 = cameraPosTemp.m00;
        cameraPos.m01 = cameraPosTemp.m01;
        cameraPos.m02 = cameraPosTemp.m02;
        cameraPos.m03 = cameraPosTemp.m03;
        cameraPos.m10 = cameraPosTemp.m10;
        cameraPos.m11 = cameraPosTemp.m11;
        cameraPos.m12 = cameraPosTemp.m12;
        cameraPos.m13 = cameraPosTemp.m13;
        cameraPos.m20 = cameraPosTemp.m20;
        cameraPos.m21 = cameraPosTemp.m21;
        cameraPos.m22 = cameraPosTemp.m22;
        cameraPos.m23 = cameraPosTemp.m23;
        cameraPos.m30 = cameraPosTemp.m30;
        cameraPos.m31 = cameraPosTemp.m31;
        cameraPos.m32 = cameraPosTemp.m32;
        cameraPos.m33 = cameraPosTemp.m33;
        #endregion
        
        cameraPos = cameraPos.transpose;

        // TRANSLATION: invert y 
        Vector3 posTemp = new Vector3(cameraPos.m03, -cameraPos.m13, cameraPos.m23);
        // ROTATION: invert x and z axis
        Quaternion rotTemp = new Quaternion(-cameraPos.rotation.x, cameraPos.rotation.y, -cameraPos.rotation.z, cameraPos.rotation.w);
        
        if (sendTransformToCam)
        {
            Camera.main.transform.rotation = rotTemp;
            Camera.main.transform.position = posTemp;
        }
        */
        
        MeshInfo meshInfo;
        SharedMeshData currentBuffer = sharedMeshBuffers[currentBufferNumber];
        
        currentBuffer.sharedMemories[0].Lock();
        currentBuffer.sharedMemories[0].accessor.Read<MeshInfo>(0, out meshInfo);
        currentBuffer.sharedMemories[0].Unlock();

        if (meshInfo.meshId < 0)
        {
            return;
        }
        else if (clientReady == false && meshInfo.meshId == 1)
        {
            UIManager.WriteToLogger("Shared memory setup complete.");
            UIManager.SetConnectionState(2, true);
            Debug.Log("Setup complete.");
            clientReady = true;
        }
            
        // Debug.Log("New mesh update on mesh " + meshInfo.meshId);

        Vector3[] vertices = new Vector3[meshInfo.numVertices];
        currentBuffer.sharedMemories[1].Lock();
        currentBuffer.sharedMemories[1].accessor.ReadArray<Vector3>(0, vertices, 0, meshInfo.numVertices);
        currentBuffer.sharedMemories[1].Unlock();

        // TODO NORMALS
        
        Vector3[] normals = new Vector3[meshInfo.numVertices];
        currentBuffer.sharedMemories[1].Lock();
        currentBuffer.sharedMemories[1].accessor.ReadArray<Vector3>(0, normals, 0, meshInfo.numVertices);
        currentBuffer.sharedMemories[1].Unlock();

        int[] faceIndices = new int[meshInfo.numFaceIndices];
        currentBuffer.sharedMemories[3].Lock();
        currentBuffer.sharedMemories[3].accessor.ReadArray<int>(0, faceIndices, 0, meshInfo.numFaceIndices);
        currentBuffer.sharedMemories[3].Unlock();
        
        Vector4[] colors = new Vector4[meshInfo.numVertices];
        currentBuffer.sharedMemories[4].Lock();
        currentBuffer.sharedMemories[4].accessor.ReadArray<Vector4>(0, colors, 0, meshInfo.numVertices);
        currentBuffer.sharedMemories[4].Unlock();
        
        // Update active mesh
        if (activeMeshes.ContainsKey(meshInfo.meshId))
        {
            UpdateMesh(activeMeshes[meshInfo.meshId], vertices, normals, faceIndices, colors);
        }
        // Create new mesh
        else
        {
            GameObject newGameObject = Instantiate(prefab, parent.transform);
            newGameObject.name = "Mesh" + meshInfo.meshId;
            UpdateMesh(newGameObject, vertices, normals, faceIndices, colors);
            activeMeshes.Add(meshInfo.meshId, newGameObject);
            // Meshes come in upside down
            newGameObject.transform.localScale = new Vector3(1,-1,1);
        }
        
        // mark update as read
        currentBuffer.sharedMemories[0].Lock();
        MeshInfo answer = new MeshInfo();
        answer.meshId = -1;
        currentBuffer.sharedMemories[0].accessor.Write<MeshInfo>(0, ref answer);
        currentBuffer.sharedMemories[0].Unlock();
        
        currentBufferNumber = (currentBufferNumber + 1) % NUMBUFFERS;
    }

    public void OnDestroy()
    {
        if (sharedMemoryInitialized == false)
        {
            return;
        }
        
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < sharedMeshBuffers[i].sharedMemories.Length; j++)
            {
                sharedMeshBuffers[i].sharedMemories[j].Destroy();
            }
        }
    }

    private class SharedMeshData
    {
        public SharedMemoryAccess[] sharedMemories;

        public SharedMeshData(int bufferNumber)
        {
            sharedMemories = new SharedMemoryAccess[5];
            
            sharedMemories[0] = new SharedMemoryAccess(meshInfoMutexName + bufferNumber, meshInfoFileName + bufferNumber);
            sharedMemories[1] = new SharedMemoryAccess(verticesMutexName + bufferNumber, verticesFileName + bufferNumber);
            sharedMemories[2] = new SharedMemoryAccess(normalsMutexName + bufferNumber, normalsFileName + bufferNumber);
            sharedMemories[3] = new SharedMemoryAccess(facesMutexName + bufferNumber, facesFileName + bufferNumber);
            sharedMemories[4] = new SharedMemoryAccess(colorsMutexName + bufferNumber, colorsFileName + bufferNumber);
        }
    }
}


