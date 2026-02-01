from django.contrib import admin

from .models import Drug, Scientific_Drug, Company, Ingredient, Drug_Form, Dose_Unit
# Register your models here.

admin.site.register(Drug)
admin.site.register(Scientific_Drug)
admin.site.register(Company)
admin.site.register(Dose_Unit)
admin.site.register(Ingredient)
admin.site.register(Drug_Form)
