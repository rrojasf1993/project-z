import sys
from Services.OcrService import OcrService


def main():
    """Simple console app to use OCR service"""
    ocr = OcrService()

    print("OCR Service Console App")
    print("-" * 40)

    while True:
        print("\nOptions:")
        print("1. Process image file")
        print("2. Exit")

        choice = input("\nEnter your choice (1-2): ").strip()

        if choice == "1":
            image_path = input("Enter image file path: ").strip()
            try:
                result = ocr.extractText(image_path)
                print("\nExtracted Text:")
                print(result)
            except Exception as e:
                print(f"Error: {e}")
        elif choice == "2":
            print("Exiting...")
            break
        else:
            print("Invalid choice. Please try again.")


if __name__ == "__main__":
    main()