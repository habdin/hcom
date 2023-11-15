from django.views.generic import ListView
from .models import Drug


class DrugListView(ListView):
    model = Drug
    context_object_name = 'drugs'
