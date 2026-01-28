import cv2
import matplotlib.pyplot as plt
import numpy as np

class MatplotlibUtil:
    @staticmethod
    def renderStepWithImage(imgage:np.ndarray, title:str, cmap:np.ndarray=None):
        plt.figure(plt.figure(figsize=(10, 10)))
        if cmap is not None:
            plt.imshow(imgage, cmap=cmap)
        else:
            plt.imshow(cv2.cvtColor(imgage, cv2.COLOR_BGR2RGB))
        plt.title(title)
        plt.axis('off')
        plt.show()