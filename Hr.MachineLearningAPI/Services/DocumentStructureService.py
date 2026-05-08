import gc
import logging

from paddleocr import PPStructureV3

from Models.ExtractTableRequest import ExtractTableRequest


class DocumentStructureService:
    documentExtractionEngine:PPStructureV3
    def __init__(self):
        self.documentExtractionEngine=PPStructureV3(#use_doc_unwarping=True,
                                                        use_table_recognition=True)
                                                     #  use_chart_recognition=True,
                                                      # use_formula_recognition=True)
        logging.basicConfig(
            format='%(asctime)s - %(levelname)s - %(message)s',
            datefmt='%Y-%m-%d %H:%M:%S',
            filename="./Document-Features-Extractor.log",
            filemode="a+"
        )
    def extract_table_data(self,imagePath:str)->object:
        try:
            rawResult=self.documentExtractionEngine.predict(imagePath)
            if (rawResult is not None):
                print(rawResult)
        except Exception as e:
            print(e)
        finally:
            gc.collect()
