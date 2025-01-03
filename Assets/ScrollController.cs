using System.Collections.Generic;
using UnityEngine;

public class ProductScrollManager : MonoBehaviour
{
    public List<GameObject> productPrefabs; // List of product prefabs
    public Transform previousPoint;
    public Transform mainPoint;
    public Transform nextPoint;
    public float scrollSpeed = 5f; // Speed of the scrolling

    private List<GameObject> products = new List<GameObject>(); // Instantiated products
    private int currentIndex = 0;
    private bool isScrolling = false;

    private void Start()
    {
        InstantiateProducts();
        UpdateProductPositions();
    }

    private void Update()
    {
        HandleScrollInput();
    }

    private void InstantiateProducts()
    {
        foreach (var prefab in productPrefabs)
        {
            GameObject product = Instantiate(prefab, mainPoint.position, Quaternion.identity, transform);
            products.Add(product);
            product.SetActive(false); // Initially disable all products
        }

        if (products.Count > 0)
        {
            products[0].SetActive(true); // Activate the first product as the main one
        }
    }

    private void HandleScrollInput()
    {
        // Desktop: Mouse Wheel (Vertical Scrolling)
        if (Input.GetAxis("Mouse ScrollWheel") > 0f && !isScrolling)
        {
            ScrollToPrevious(); // Scroll up
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f && !isScrolling)
        {
            ScrollToNext(); // Scroll down
        }

        // Mobile: Touch Input (Vertical Swiping)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved && !isScrolling)
            {
                if (touch.deltaPosition.y > 0)
                {
                    ScrollToNext(); // Swipe down
                }
                else if (touch.deltaPosition.y < 0)
                {
                    ScrollToPrevious(); // Swipe up
                }
            }
        }
    }

    private void ScrollToNext()
    {
        if (currentIndex < products.Count - 1)
        {
            currentIndex++;
            StartCoroutine(SmoothScroll());
        }
    }

    private void ScrollToPrevious()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            StartCoroutine(SmoothScroll());
        }
    }

    private System.Collections.IEnumerator SmoothScroll()
    {
        isScrolling = true;

        Vector3 targetPositionMain = mainPoint.position;
        Vector3 targetPositionPrevious = previousPoint.position;
        Vector3 targetPositionNext = nextPoint.position;

        GameObject currentProduct = products[currentIndex];
        GameObject previousProduct = currentIndex > 0 ? products[currentIndex - 1] : null;
        GameObject nextProduct = currentIndex < products.Count - 1 ? products[currentIndex + 1] : null;

        foreach (var product in products) product.SetActive(false); // Deactivate all products
        if (previousProduct != null) previousProduct.SetActive(true);
        if (currentProduct != null) currentProduct.SetActive(true);
        if (nextProduct != null) nextProduct.SetActive(true);

        float elapsedTime = 0f;
        while (elapsedTime < 0.3f)
        {
            if (currentProduct != null)
                currentProduct.transform.position = Vector3.Lerp(currentProduct.transform.position, targetPositionMain, elapsedTime);
            if (previousProduct != null)
                previousProduct.transform.position = Vector3.Lerp(previousProduct.transform.position, targetPositionPrevious, elapsedTime);
            if (nextProduct != null)
                nextProduct.transform.position = Vector3.Lerp(nextProduct.transform.position, targetPositionNext, elapsedTime);

            elapsedTime += Time.deltaTime * scrollSpeed;
            yield return null;
        }

        UpdateProductPositions();
        isScrolling = false;
    }

    private void UpdateProductPositions()
    {
        for (int i = 0; i < products.Count; i++)
        {
            if (i < currentIndex)
            {
                products[i].transform.position = previousPoint.position;
            }
            else if (i == currentIndex)
            {
                products[i].transform.position = mainPoint.position;
            }
            else if (i > currentIndex)
            {
                products[i].transform.position = nextPoint.position;
            }

            products[i].SetActive(i == currentIndex || i == currentIndex - 1 || i == currentIndex + 1);
        }
    }
}
