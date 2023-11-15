from django.db import models


class Dummy(models.Model):
    name = models.CharField("name", max_length=60, blank=False, unique=True)
    category = models.CharField("category", max_length=60)

    def __str__(self):
        return self.name
    
    class Meta:
       verbose_name_plural = 'Dummies' 

