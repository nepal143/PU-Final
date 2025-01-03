using System.Collections.Generic;
using UnityEngine;
using TMPro; // Add this namespace for TextMeshPro

public class ProductScrollManager : MonoBehaviour
{
    [System.Serializable]
    public class ProductData
    {
        public GameObject prefab; // Product prefab
        public string name; // Product name
        public float price; // Product price
    }

    public List<ProductData> productsData; // List of product data
    public Transform previousPoint;
    public Transform mainPoint;
    public Transform nextPoint;
    public float scrollSpeed = 5f;

    private List<GameObject> products = new List<GameObject>(); // Instantiated product objects
    private int currentIndex = 0;
    private bool isScrolling = false;

    // Public references for the TextMeshProUGUI components (global)
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI priceText;

    private void Start()
    {
        InstantiateProducts();
        UpdateProductPositions();
        
        // Ensure the first product's name and price are displayed on startup
        if (nameText != null && priceText != null && productsData.Count > 0)
        {
            nameText.text = productsData[0].name; // Set name for the first product
            priceText.text = $"${productsData[0].price:F2}"; // Set price for the first product
        }
    }

    private void Update()
    {
        HandleScrollInput();
    }

    private void InstantiateProducts()
    {
        foreach (var data in productsData)
        {
            // Instantiate each product
            GameObject product = Instantiate(data.prefab, mainPoint.position, Quaternion.identity, transform);
            products.Add(product);

            // Disable all products initially
            product.SetActive(false);
        }

        if (products.Count > 0)
        {
            products[0].SetActive(true); // Activate the first product as the main one
        }
    }

    private void HandleScrollInput()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f && !isScrolling)
        {
            ScrollToPrevious();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f && !isScrolling)
        {
            ScrollToNext();
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved && !isScrolling)
            {
                if (touch.deltaPosition.y > 0)
                {
                    ScrollToNext();
                }
                else if (touch.deltaPosition.y < 0)
                {
                    ScrollToPrevious();
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

        foreach (var product in products) product.SetActive(false);
        if (previousProduct != null) previousProduct.SetActive(true);
        if (currentProduct != null) currentProduct.SetActive(true);
        if (nextProduct != null) nextProduct.SetActive(true);

        // Update name and price for the currently active product
        if (nameText != null && priceText != null)
        {
            nameText.text = productsData[currentIndex].name; // Set product name
            priceText.text = $"${productsData[currentIndex].price:F2}"; // Set product price (formatted)
        }

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
